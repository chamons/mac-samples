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
	class OneShotLocation : NSObject, ICLLocationManagerDelegate
	{
		CLLocationManager locationManager;
		Func<CLLocation [], Task> Action;

		public void Start (Func<CLLocation [], Task> action)
		{
			Action = action;

			locationManager = new CLLocationManager ();
			locationManager.Delegate = this;
			locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
			locationManager.StartUpdatingLocation ();
		}

		[Export ("locationManager:didUpdateLocations:")]
		public async void LocationsUpdated (CLLocationManager manager, CLLocation [] locations)
		{
			locationManager.StopUpdatingLocation ();
			await Action (locations);
		}
	}
}