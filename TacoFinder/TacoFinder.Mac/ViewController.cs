using System;
using System.Collections.Generic;
using AppKit;
using CoreLocation;
using Foundation;

namespace TacoFinder.Mac
{
	public partial class ViewController : NSViewController, ICLLocationManagerDelegate
	{
		CLLocationManager locationManager;
		TacoLib.TacoFinder finder;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			locationManager = new CLLocationManager ();
			locationManager.Delegate = this;
			locationManager.DesiredAccuracy = CLLocation.AccuracyBest;

			locationManager.StartUpdatingLocation ();
		}

		[Export ("locationManager:didUpdateLocations:")]
		public void LocationsUpdated (CLLocationManager manager, CLLocation [] locations)
		{
			locationManager.StopUpdatingLocation ();
			if (locations.Length > 0)
				Find (locations [0].Coordinate.Latitude, locations [0].Coordinate.Longitude);
		}

		public async void Find (double latitude, double longitude)
		{
			finder = new TacoLib.TacoFinder ();
			IEnumerable<TacoLib.TacoLocation> list = await finder.Find (latitude, longitude);
			foreach (var v in list)
				Console.WriteLine (v.Name);
		}
	}
}
