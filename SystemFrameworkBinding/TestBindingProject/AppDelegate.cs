using AppKit;
using Foundation;

namespace TestBindingProject
{
	[Register ("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		public AppDelegate ()
		{
		}

		public override void DidFinishLaunching (NSNotification notification)
		{
			ObjCRuntime.Dlfcn.dlopen ("/System/Library/Frameworks/CryptoTokenKit.framework/CryptoTokenKit", 0);
			var manager = CryptoTokenKit.TKSmartCardSlotManager.DefaultManager;
			System.Console.WriteLine (manager != null);
		}

		public override void WillTerminate (NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}
