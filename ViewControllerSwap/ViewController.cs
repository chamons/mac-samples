using System;

using AppKit;
using Foundation;

namespace ViewControllerSwap
{
	public partial class ViewController : NSViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		NSViewController firstController;
		NSViewController secondController;
		NSViewController currentController;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			firstController = new FirstViewController ();
			AddChildViewController (firstController);

			secondController = new SecondViewController ();
			AddChildViewController (secondController);

			Show (firstController);

			// This is a hack, but convenient for a quick example 
			AppDelegate.RootViewController = this;
		}

		void Show (NSViewController controller)
		{
			if (currentController != null)
				currentController.View.RemoveFromSuperview ();

			controller.View.Frame = View.Frame;
			View.AddSubview (controller.View);

			currentController = controller;
		}

		public void Swap (bool first)
		{
			if (first)
				Show (firstController);
			else
				Show (secondController);
		}
	}
}
