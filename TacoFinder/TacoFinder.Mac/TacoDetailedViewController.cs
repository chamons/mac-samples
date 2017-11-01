using System;

using Foundation;
using AppKit;

namespace TacoFinder.Mac
{
	public partial class TacoDetailedViewController : NSViewController
	{
		public TacoDetailedViewController (IntPtr handle) : base (handle)
		{
		}

		public void SetSelection (TacoLib.TacoLocation location)
		{
			Name.StringValue = location.Name;

		}
	}
}
