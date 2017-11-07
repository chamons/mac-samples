using System;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;

namespace TacoFinder
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