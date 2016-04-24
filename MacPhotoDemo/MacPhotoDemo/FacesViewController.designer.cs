// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MacPhotoDemo
{
	[Register ("FacesViewController")]
	partial class FacesViewController
	{
		[Outlet]
		AppKit.NSTextView DetailView { get; set; }

		[Outlet]
		AppKit.NSButton PickPhotoButton { get; set; }

		[Outlet]
		AppKit.NSImageView ThePhoto { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PickPhotoButton != null) {
				PickPhotoButton.Dispose ();
				PickPhotoButton = null;
			}

			if (ThePhoto != null) {
				ThePhoto.Dispose ();
				ThePhoto = null;
			}

			if (DetailView != null) {
				DetailView.Dispose ();
				DetailView = null;
			}
		}
	}
}
