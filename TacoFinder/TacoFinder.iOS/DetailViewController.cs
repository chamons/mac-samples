using System;

using UIKit;

namespace TacoFinder.iOS
{
	public partial class DetailViewController : UIViewController
	{
		public object DetailItem { get; set; }

		protected DetailViewController (IntPtr handle) : base (handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void SetDetailItem (object newDetailItem)
		{
			if (DetailItem != newDetailItem)
			{
				DetailItem = newDetailItem;

				// Update the view
				ConfigureView ();
			}
		}

		void ConfigureView ()
		{
			// Update the user interface for the detail item
			if (IsViewLoaded && DetailItem != null)
				detailDescriptionLabel.Text = DetailItem.ToString ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

