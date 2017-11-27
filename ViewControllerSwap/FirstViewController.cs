using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace ViewControllerSwap
{
	public partial class FirstViewController : AppKit.NSViewController
	{
		#region Constructors

		// Called when created from unmanaged code
		public FirstViewController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public FirstViewController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}

		// Call to load from the XIB/NIB file
		public FirstViewController () : base ("FirstView", NSBundle.MainBundle)
		{
			Initialize ();
		}

		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		//strongly typed view accessor
		public new FirstView View
		{
			get
			{
				return (FirstView)base.View;
			}
		}
	}
}
