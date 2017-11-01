// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TacoFinder.Mac
{
	[Register ("TacoDetailedViewController")]
	partial class TacoDetailedViewController
	{
		[Outlet]
		MapKit.MKMapView MapView { get; set; }

		[Outlet]
		AppKit.NSTextField Name { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Name != null) {
				Name.Dispose ();
				Name = null;
			}

			if (MapView != null) {
				MapView.Dispose ();
				MapView = null;
			}
		}
	}
}
