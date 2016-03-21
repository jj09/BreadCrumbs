using System;

using Foundation;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using BreadCrumbs.Shared.Models;
using BreadCrumbs.Shared.Helpers;
using BreadCrumbs.Shared.ViewModels;

namespace BreadCrumbs.iOS
{
	public partial class ViewController : UIViewController
	{
		MainViewModel ViewModel;

		public ViewController (IntPtr handle) : base (handle)
		{
			ViewModel = new MainViewModel();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			PlacesTableView.Source = new PlacesTableSource(ViewModel);
            placeNameTextField.Placeholder = "Name of the place";
            placeNameTextField.ShouldReturn += (textField) =>
            {
                if (textField == placeNameTextField)
                {
                    SavePlaceAndClearTextField();
                    return false;
                }
                return true;
            };

            saveButton.TouchUpInside += (sender, e) =>
            {
                SavePlaceAndClearTextField();
            };
		}

        private void SavePlaceAndClearTextField()
        {
            var name = this.placeNameTextField.Text;
            ViewModel.SaveAsync(name).ContinueWith((result) => {
                PlacesTableView.ReloadData();
            }, TaskScheduler.FromCurrentSynchronizationContext());
            this.placeNameTextField.Text = "";
        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
            PlacesTableView.ReloadData();
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

			cell.TextLabel.Text = item.DisplayName;

			return cell;
		}

		async public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            // navigate to place
            var place = ViewModel.SavedPlaces.ElementAt(indexPath.Row);
            var placeCoordinates = place.Coordinates;

            string mapsUrl = "";
            if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl("comgooglemaps://")))
            {
                var currentLocationCoordinates = await LocationHelper.GetCurrentLocation();
                mapsUrl = $"comgooglemaps://?sq={currentLocationCoordinates.Lat},{currentLocationCoordinates.Long}&dq={placeCoordinates.Lat},{placeCoordinates.Long}&directionsmode=walking";
            }
            else
            {
                mapsUrl = $"http://maps.apple.com/?q={placeCoordinates.Lat},{placeCoordinates.Long}";
            }
            

            UIApplication.SharedApplication.OpenUrl(new NSUrl(mapsUrl));
		}

        // deleting from the list
        // https://developer.xamarin.com/guides/ios/user_interface/tables/part_4_-_editing/#Swipe_to_Delete
        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    // remove the item from the underlying data source
                    ViewModel.Remove(ViewModel.SavedPlaces.ElementAt(indexPath.Row));
                    // delete the row from the table
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
                    break;
                case UITableViewCellEditingStyle.None:
                    break;
            }
        }
        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true; // return false if you wish to disable editing for a specific indexPath or for all rows
        }
    }
}

