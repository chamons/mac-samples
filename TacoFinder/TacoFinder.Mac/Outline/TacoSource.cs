using System;
using System.Linq;
using System.Threading.Tasks;
using AppKit;
using CoreLocation;
using Foundation;
using TacoLib;

namespace TacoFinder.Mac.Outline
{
	class TacoSource : NSObject, INSOutlineViewDelegate, INSOutlineViewDataSource
	{
		class NSObjectWrapper<T> : NSObject
		{
			public T Value { get; private set; }

			public NSObjectWrapper (T value)
			{
				Value = value;
			}
		}

		TacoOptions Options;

		public TacoSource (TacoOptions options)
		{
			Options = options;
		}

		static TacoLocation UnwrapLocation (NSObject item)
		{
			return ((NSObjectWrapper<TacoLocation>)item).Value;
		}

		public event EventHandler<TacoLocation> SelectionChanged;
		void OnChanged (TacoLocation location) => SelectionChanged?.Invoke (this, location);

		[Export ("outlineView:numberOfChildrenOfItem:")]
		public nint GetChildrenCount (NSOutlineView outlineView, NSObject item)
		{
			if (item == null)
				return Options.Brands.Count ();
			else
				return Options.GetLocations ((NSString)item).Count ();
		}

		[Export ("outlineView:child:ofItem:")]
		public NSObject GetChild (NSOutlineView outlineView, nint childIndex, NSObject item)
		{
			// Root Element
			if (item == null)
			{
				return (NSString)Options.Brands.ElementAt ((int)childIndex);
			}
			else
			{
				string brand = (NSString)item;
				return new NSObjectWrapper<TacoLocation> (Options.GetLocations (brand).ElementAt ((int)childIndex));
			}
		}

		[Export ("outlineView:isItemExpandable:")]
		public bool ItemExpandable (NSOutlineView outlineView, NSObject item)
		{
			return item is NSString;
		}

		string GetName (NSObject item)
		{
			if (item is NSString)
				return (string)(NSString)item;
			else
				return UnwrapLocation (item).Name;
		}

		[Export ("outlineView:objectValueForTableColumn:byItem:")]
		public NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
		{
			return item;
		}

		[Export ("outlineView:viewForTableColumn:item:")]
		public NSView GetView (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
		{
			var view = (NSTableCellView)outlineView.MakeView ("HeaderCell", this);
			view.TextField.StringValue = GetName (item);
			view.TextField.TextColor = NSColor.Black;
			return view;
		}

		[Export ("outlineViewSelectionDidChange:")]
		public void SelectionDidChange (NSNotification notification)
		{
			NSOutlineView view = (NSOutlineView)notification.Object;
			var selectedItem = view.ItemAtRow (view.SelectedRow);
			if (!(selectedItem is NSString)) // If specific location
			{
				var location = UnwrapLocation (selectedItem);
				OnChanged (location);
			}
			else // Is top level brand, select first location inside
			{
				// NSSourceList is a NSOutlineView with children as the next element if expanded
				view.ExpandItem (selectedItem);
				view.SelectRow (view.SelectedRow + 1, false);
			}
		}
	}
}