using System;
using System.Linq;
using System.Threading.Tasks;
using AppKit;
using CoreLocation;
using Foundation;
using TacoFinder.Mac.Outline;
using TacoLib;

namespace TacoFinder.Mac
{
	public partial class ViewController : NSViewController
	{
		OneShotLocation locationFinder = new OneShotLocation ();
		TacoSource source;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			DetailView.WantsLayer = true;

			locationFinder.Start (async locations => {
				if (locations.Length > 0)
					await Find (locations [0].Coordinate.Latitude, locations [0].Coordinate.Longitude);
			});

			SetBusy ();
		}

		void SetBusy ()
		{
			SourceList.Delegate = BusySource.Instance;
			SourceList.DataSource = BusySource.Instance;
		}

		public async Task Find (double latitude, double longitude)
		{
			var finder = new TacoLib.TacoFinder ();
			var options = await finder.Find (latitude, longitude);

			source = new TacoSource (options);
			SourceList.Delegate = source;
			SourceList.DataSource = source;
			source.SelectionChanged += (sender, location) => SetDetailView (location);

			SourceList.SelectRow (0, false);
		}

		NSStackView stack;
		NSTextView label;
		MapKit.MKMapView map;
		const int Padding = 10;
		void SetDetailView (TacoLocation location)
		{
			if (stack == null)
			{
				map = new MapKit.MKMapView ();

				label = new NSTextView ()
				{
					DrawsBackground = false,
					Editable = false,
					Selectable = false,
					Alignment = NSTextAlignment.Center,
					Value = "Location",
				};

				stack = new NSStackView ()
				{
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
			SetMap (location);
		}

		void SetMap (TacoLocation location)
		{
			var region = MapKit.MKCoordinateRegion.FromDistance (new CLLocationCoordinate2D (location.Latitude, location.Longitude), 1000, 1000);
			map.SetRegion (region, true);

			map.RemoveAnnotations (map.Annotations);
			map.AddAnnotation (new TacoAnnotation (location));
		}

		class TacoAnnotation : MapKit.MKAnnotation
		{
			CLLocationCoordinate2D coordinate;

			public TacoAnnotation (TacoLib.TacoLocation location)
			{
				coordinate = new CLLocationCoordinate2D (location.Latitude, location.Longitude);
			}

			public override CLLocationCoordinate2D Coordinate => coordinate;
		}
	}
}
