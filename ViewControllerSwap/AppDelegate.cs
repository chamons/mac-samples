using AppKit;
using Foundation;

namespace ViewControllerSwap
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : NSApplicationDelegate
	{
		public AppDelegate ()
		{
		}


		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender) => true;

		// This is a hack, but convenient for a quick example 
		public static ViewController RootViewController;

		partial void OnFirstMenu (NSObject sender)
		{
			RootViewController.Swap (true);	
		}

		partial void OnSecondMenu (Foundation.NSObject sender)
		{
			RootViewController.Swap (false);
		}
	}
}
