﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace ViewControllerSwap
{
	public partial class FirstView : AppKit.NSView
	{
		#region Constructors

		// Called when created from unmanaged code
		public FirstView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public FirstView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}

		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion
	}
}
