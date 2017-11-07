using System;
using System.Collections.Generic;

using UIKit;
using Foundation;
using System.Threading.Tasks;
using TacoLib;

namespace TacoFinder.iOS
{
	public partial class MasterViewController : UITableViewController
	{
		public DetailViewController DetailViewController { get; set; }

		TacoDataSource dataSource;
		OneShotLocation locationFinder = new OneShotLocation ();

		protected MasterViewController (IntPtr handle) : base (handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = NSBundle.MainBundle.LocalizedString ("Master", "Master");
			DetailViewController = (DetailViewController)((UINavigationController)SplitViewController.ViewControllers[1]).TopViewController;

			locationFinder.Start (async locations => {
				if (locations.Length > 0)
					await Find (locations [0].Coordinate.Latitude, locations [0].Coordinate.Longitude);
			});
		}

		public async Task Find (double latitude, double longitude)
		{
			var finder = new TacoLib.TacoFinder ();
			var options = await finder.Find (latitude, longitude);
			TableView.Source = dataSource = new TacoDataSource (this, options);
		}

		public override void ViewWillAppear (bool animated)
		{
			ClearsSelectionOnViewWillAppear = SplitViewController.Collapsed;
			base.ViewWillAppear (animated);
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "showDetail")
			{
				var controller = (DetailViewController)((UINavigationController)segue.DestinationViewController).TopViewController;
				var indexPath = TableView.IndexPathForSelectedRow;
				var item = dataSource.Objects[indexPath.Row];

				controller.SetDetailItem (item);
				controller.NavigationItem.LeftBarButtonItem = SplitViewController.DisplayModeButtonItem;
				controller.NavigationItem.LeftItemsSupplementBackButton = true;
			}
		}

		class TacoDataSource : UITableViewSource
		{
			static readonly NSString CellIdentifier = new NSString ("Cell");

			readonly MasterViewController Controller;
			TacoOptions Options;

			public TacoDataSource (MasterViewController controller, TacoOptions options)
			{
				controller = controller;
				Options = options;

			}

			public IList<object> Objects
			{
				get { return objects; }
			}

			// Customize the number of sections in the table view.
			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return objects.Count;
			}

			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (CellIdentifier, indexPath);

				cell.TextLabel.Text = objects[indexPath.Row].ToString ();

				return cell;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath) => false;


			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
					Controller.DetailViewController.SetDetailItem (objects[indexPath.Row]);
			}
		}
	}
}
