using System;
using AppKit;
using Foundation;

namespace TacoFinder.Mac.Outline
{
	class BusySource : NSObject, INSOutlineViewDelegate, INSOutlineViewDataSource
	{
		public static BusySource Instance = new BusySource ();

		[Export ("outlineView:numberOfChildrenOfItem:")]
		public nint GetChildrenCount (NSOutlineView outlineView, NSObject item) => 1;

		[Export ("outlineView:child:ofItem:")]
		public NSObject GetChild (NSOutlineView outlineView, nint childIndex, NSObject item) => (NSString)"";

		[Export ("outlineView:isItemExpandable:")]
		public bool ItemExpandable (NSOutlineView outlineView, NSObject item) => false;

		[Export ("outlineView:objectValueForTableColumn:byItem:")]
		public NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item) => (NSString)"";

		[Export ("outlineView:viewForTableColumn:item:")]
		public NSView GetView (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
		{
			var view = (NSTableCellView)outlineView.MakeView ("HeaderCell", this);
			view.TextField.StringValue = "Loading...";
			return view;
		}
	}
}