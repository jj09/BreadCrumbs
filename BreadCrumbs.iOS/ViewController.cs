using System;

using UIKit;
using BreadCrumbs.Shared.ViewModels;
using System.Linq;
using Foundation;
using BreadCrumbs.Shared.Models;

namespace BreadCrumbs.iOS
{
	public partial class ViewController : UIViewController
	{
		MainViewModel ViewModel;

		public ViewController (IntPtr handle) : base (handle)
		{
			ViewModel = new MainViewModel();

			this.saveButton.TouchUpInside += (object sender, EventArgs e) => 
			{
				var name = this.placeNameTextField.Text;
				ViewModel.SaveAsync(name, new Coordinates(1,2));
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			PlacesTableView.Source = new PlacesTableSource(ViewModel);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
//			TableView.ReloadData();
		}
	}

	public class PlacesTableSource : UITableViewSource
	{
		string CellIdentifier = "TableCell";

		MainViewModel ViewModel;


		public PlacesTableSource (MainViewModel vm)
		{
			ViewModel = vm;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return ViewModel.SavedPlaces.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);
			var item = ViewModel.SavedPlaces.ElementAt(indexPath.Row);

			if (cell == null)
			{
				cell = new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier);
			}

			cell.TextLabel.Text = item.Name;

			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			// TODO: navigate to place
		}
	}
}

