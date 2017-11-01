using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using CoreLocation;
using Foundation;

namespace TacoFinder.Mac
{
	class BusySource : NSObject, INSOutlineViewDelegate, INSOutlineViewDataSource
	{
		[Export ("outlineView:numberOfChildrenOfItem:")]
		public nint GetChildrenCount (NSOutlineView outlineView, NSObject item) => 1;

		[Export ("outlineView:child:ofItem:")]
		public NSObject GetChild (NSOutlineView outlineView, nint childIndex, NSObject item) => (NSString)"";

		[Export ("outlineView:isItemExpandable:")]
		public bool ItemExpandable (NSOutlineView outlineView, NSObject item) => false;

		[Export ("outlineView:objectValueForTableColumn:byItem:")]
		public NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item) => (NSString)"";

		[Export ("outlineView:viewForTableColumn:item:")]
		public NSView GetView (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
		{
			var view = (NSTableCellView)outlineView.MakeView ("HeaderCell", this);
			view.TextField.StringValue = "Loading...";
			return view;
		}
	}

	class NSObjectWrapper<T> : NSObject
	{
		public T Value { get; private set; }

		public NSObjectWrapper(T value)
		{
			Value = value;
		}
	}

	class TacoSource : NSObject, INSOutlineViewDelegate, INSOutlineViewDataSource
	{
		List<TacoLib.TacoLocation> Locations;
		public TacoSource (List<TacoLib.TacoLocation> locations)
		{
			Locations = locations;
		}

		static TacoLib.TacoLocation UnwrapLocation (NSObject item)
		{
			return ((NSObjectWrapper<TacoLib.TacoLocation>)item).Value;
		}

		public event EventHandler<TacoLib.TacoLocation> SelectionChanged;
		void OnChanged (TacoLib.TacoLocation location) => SelectionChanged?.Invoke (this, location);
		
		[Export ("outlineView:numberOfChildrenOfItem:")]
		public nint GetChildrenCount (NSOutlineView outlineView, NSObject item) => Locations.Count;

		[Export ("outlineView:child:ofItem:")]
		public NSObject GetChild (NSOutlineView outlineView, nint childIndex, NSObject item)
		{
			return new NSObjectWrapper<TacoLib.TacoLocation> (Locations [(int)childIndex]);
		}

		[Export ("outlineView:isItemExpandable:")]
		public bool ItemExpandable (NSOutlineView outlineView, NSObject item) => false;

		[Export ("outlineView:objectValueForTableColumn:byItem:")]
		public NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
		{
			var location = UnwrapLocation (item);
			return (NSString)location.Name;
		}

		[Export ("outlineView:viewForTableColumn:item:")]
		public NSView GetView (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
		{
			var location = UnwrapLocation (item);
			var view = (NSTableCellView)outlineView.MakeView ("HeaderCell", this);
			view.TextField.StringValue = location.Name;
			return view;
		}

		[Export ("outlineViewSelectionDidChange:")]
		public void SelectionDidChange (NSNotification notification)
		{
			NSOutlineView view = (NSOutlineView)notification.Object;
			int index = (int)view.SelectedRow;
			OnChanged (Locations[index]);
		}
	}

	public class TacoAnnotation : MapKit.MKAnnotation
	{
		CLLocationCoordinate2D coordinate;

		public TacoAnnotation (TacoLib.TacoLocation location)
		{
			coordinate = new CLLocationCoordinate2D (location.Latitude, location.Longitude);
		}

		public override CLLocationCoordinate2D Coordinate => coordinate;
	}

	public partial class ViewController : NSViewController, ICLLocationManagerDelegate
	{
		CLLocationManager locationManager;
		BusySource busySource = new BusySource ();
		TacoSource source;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			DetailView.WantsLayer = true;

			locationManager = new CLLocationManager ();
			locationManager.Delegate = this;
			locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
			locationManager.StartUpdatingLocation ();
			SetBusy ();
		}

		void SetBusy ()
		{
			SourceList.Delegate = busySource;
			SourceList.DataSource = busySource;
		}

		NSStackView stack;
		NSTextView label;
		MapKit.MKMapView map;
		const int Padding = 10;
		void SetDetailView (TacoLib.TacoLocation location)
		{
			if (stack == null) {
				map = new MapKit.MKMapView () {
				};

				label = new NSTextView () {
					DrawsBackground = false,
					Editable = false,
					Selectable = false,
					Alignment = NSTextAlignment.Center,
					Value = "Location",
				};

				stack = new NSStackView () {
					Orientation = NSUserInterfaceLayoutOrientation.Vertical,
					TranslatesAutoresizingMaskIntoConstraints = false
				};

				label.HeightAnchor.ConstraintEqualToConstant (20).Active = true;

				DetailView.AddSubview (stack);

				stack.LeadingAnchor.ConstraintEqualToAnchor (DetailView.LeadingAnchor, Padding).Active = true;
				stack.TrailingAnchor.ConstraintEqualToAnchor (DetailView.TrailingAnchor, -Padding).Active = true;
				stack.TopAnchor.ConstraintEqualToAnchor (DetailView.TopAnchor, Padding).Active = true;
				stack.BottomAnchor.ConstraintEqualToAnchor (DetailView.BottomAnchor, -Padding).Active = true;

				stack.AddView (label, NSStackViewGravity.Center);
				stack.AddView (map, NSStackViewGravity.Center);
			}

			label.Value = location.Name;
			var mapLocation = new CLLocation (location.Latitude, location.Longitude);
			var region = MapKit.MKCoordinateRegion.FromDistance (mapLocation.Coordinate, 1000, 1000);
			map.SetRegion (region, true);
			map.RemoveAnnotations (map.Annotations);
			map.AddAnnotation (new TacoAnnotation (location));
		}

		[Export ("locationManager:didUpdateLocations:")]
		public async void LocationsUpdated (CLLocationManager manager, CLLocation [] locations)
		{
			locationManager.StopUpdatingLocation ();
			if (locations.Length > 0)
				await Find (locations [0].Coordinate.Latitude, locations [0].Coordinate.Longitude);
		}


		public async Task Find (double latitude, double longitude)
		{
			var finder = new TacoLib.TacoFinder ();
			var list = await finder.Find (latitude, longitude);

			source = new TacoSource (list);
			SourceList.Delegate = source;
			SourceList.DataSource = source;
			source.SelectionChanged += (sender, location) => SetDetailView (location);

			SourceList.SelectRow (0, false);
		}
	}
}
