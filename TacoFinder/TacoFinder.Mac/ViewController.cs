using System;
using System.Collections.Generic;
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

	public partial class ViewController : NSViewController, ICLLocationManagerDelegate
	{
		CLLocationManager locationManager;
		BusySource busySource = new BusySource ();
		TacoSource source;
		TacoDetailedViewController detailedController;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var mainStoryboard = NSStoryboard.FromName ("Main", null);

			detailedController = (TacoDetailedViewController)mainStoryboard.InstantiateControllerWithIdentifier ("TacoDetailedViewController");
			DetailedView.AddSubview (detailedController.View);

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
			source.SelectionChanged += (sender, location) => {
				detailedController.SetSelection (location);
			};

			SourceList.SelectRow (0, false);
		}
	}
}
