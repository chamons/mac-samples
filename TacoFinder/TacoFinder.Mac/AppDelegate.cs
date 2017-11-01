using AppKit;
using Foundation;

namespace TacoFinder.Mac
{
	[Register ("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender) => true;

		public override void DidFinishLaunching (NSNotification notification)
		{
			NSUserDefaults.StandardUserDefaults.SetBool (true, "NSViewShowAlignmentRects");
			NSUserDefaults.StandardUserDefaults.SetBool (true, "NSConstraintBasedLayoutVisualizeMutuallyExclusiveConstraints");
			NSUserDefaults.StandardUserDefaults.Synchronize ();
		}
	}
}
