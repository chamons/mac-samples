using System;
using System.IO;
using System.Reflection;
using AppKit;
using CoreGraphics;
using Foundation;

namespace QuickLookLibraryTest
{
	public partial class ViewController : NSViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var provider = new QuickLookLibrary.ImageProvider ();

			NSImageView left = new NSImageView (new CGRect (50, 50, 250, 250));
			left.WantsLayer = true;
			left.Layer.BorderWidth = 1;
			left.Layer.BorderColor = NSColor.Gray.CGColor;
			left.Image = LoadImage (provider.GetIconType ("/var/foo.xam"));
			View.AddSubview (left);


			NSImageView right = new NSImageView (new CGRect (350, 50, 250, 250));
			right.WantsLayer = true;
			right.Layer.BorderWidth = 1;
			right.Layer.BorderColor = NSColor.Gray.CGColor;
			right.Image = LoadImage (provider.GetIconType ("/var/foo.txt"));
			View.AddSubview (right);
		}

		NSImage LoadImage (QuickLookLibrary.Icons icon)
		{
			if (icon == QuickLookLibrary.Icons.Xamagon)
			{
				var assembly = Assembly.GetExecutingAssembly ();

				using (Stream stream = assembly.GetManifestResourceStream ("QuickLookLibraryTest.Xamagon.png"))
				{
					return new NSImage (NSData.FromStream (stream));
				}
			}
			return null;
		}
	}
}
