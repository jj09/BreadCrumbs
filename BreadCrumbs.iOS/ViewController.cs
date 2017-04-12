using System;

using Foundation;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using BreadCrumbs.Shared.Models;
using BreadCrumbs.Shared.Helpers;
using BreadCrumbs.Shared.ViewModels;
using CoreGraphics;

namespace BreadCrumbs.iOS
{
    public partial class ViewController : UITableViewController
	{
		private MainViewModel _viewModel;
        private SavingOverlay savingOverlay;

        public ViewController (IntPtr handle) : base (handle)
		{
			
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();

            _viewModel = ContainerHelper.Container.GetInstance<MainViewModel>();

            TableView.Source = new PlacesTableSource(_viewModel);

            _addButton.Clicked += (sender, e) =>
            {
                var addPopup = UIAlertController.Create("Enter name:", "", UIAlertControllerStyle.Alert);

                addPopup.AddTextField(x => { });

                addPopup.AddAction(UIAlertAction.Create("Add", UIAlertActionStyle.Default, x =>
                {
                    var name = addPopup.TextFields[0]?.Text;
                    SavePlaceAndClearTextField(name);
                }));

                addPopup.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, x => {}));

				PresentViewController(addPopup, true, null);
            };
		}

        private void SavePlaceAndClearTextField(string name)
        {
            ShowSavingOverlay();
            _viewModel.SaveAsync(name).ContinueWith((result) => {
                TableView.ReloadData();
                HideSavingOverlay();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ShowSavingOverlay()
        {
            var bounds = UIScreen.MainScreen.Bounds;

            // show the loading overlay on the UI thread using the correct orientation sizing
            savingOverlay = new SavingOverlay(bounds);
            View.Add(savingOverlay);
        }

        private void HideSavingOverlay()
        {
            savingOverlay.Hide();
        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
            TableView.ReloadData();
        }

    }

	public class PlacesTableSource : UITableViewSource
	{
		string CellIdentifier = "TableCell";

		private readonly MainViewModel _viewModel;
        
		public PlacesTableSource (MainViewModel vm)
		{
			_viewModel = vm;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return _viewModel.SavedPlaces.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);
			var item = _viewModel.SavedPlaces.ElementAt(indexPath.Row);

			if (cell == null)
			{
				cell = new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier);
			}

			cell.TextLabel.Text = item.DisplayName;

			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            tableView.DeselectRow(indexPath, true);

            // navigate to place
            var place = _viewModel.SavedPlaces.ElementAt(indexPath.Row);
            var placeCoordinates = place.Coordinates;

            string mapsUrl = "";
            if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl("comgooglemaps://")))
            {
                // below 2 lines doesn't work on iOS, instead of starting navigation, it just opens Google Maps and does nothing
                //var currentLocationCoordinates = await LocationHelper.GetCurrentLocation();
                //mapsUrl = $"comgooglemaps://?sq={currentLocationCoordinates.Lat},{currentLocationCoordinates.Long}&dq={placeCoordinates.Lat},{placeCoordinates.Long}&directionsmode=walking";
                mapsUrl = $"comgooglemaps://?q={placeCoordinates.Lat},{placeCoordinates.Long}";
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
                    _viewModel.Remove(_viewModel.SavedPlaces.ElementAt(indexPath.Row));
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

    public class SavingOverlay : UIView
    {
        // control declarations
        UIActivityIndicatorView activitySpinner;
        UILabel loadingLabel;

        public SavingOverlay(CGRect frame) : base(frame)
        {
            // configurable bits
            BackgroundColor = UIColor.Black;
            Alpha = 0.75f;
            AutoresizingMask = UIViewAutoresizing.All;

            nfloat labelHeight = 22;
            nfloat labelWidth = Frame.Width - 20;

            // derive the center x and y
            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            activitySpinner.Frame = new CGRect(
                centerX - (activitySpinner.Frame.Width / 2),
                centerY - activitySpinner.Frame.Height - 20,
                activitySpinner.Frame.Width,
                activitySpinner.Frame.Height);
            activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(activitySpinner);
            activitySpinner.StartAnimating();

            // create and configure the "Loading Data" label
            loadingLabel = new UILabel(new CGRect(
                centerX - (labelWidth / 2),
                centerY + 20,
                labelWidth,
                labelHeight
                ));
            loadingLabel.BackgroundColor = UIColor.Clear;
            loadingLabel.TextColor = UIColor.White;
            loadingLabel.Text = "Saving place...";
            loadingLabel.TextAlignment = UITextAlignment.Center;
            loadingLabel.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(loadingLabel);

        }

        /// <summary>
        /// Fades out the control and then removes it from the super view
        /// </summary>
        public void Hide()
        {
            UIView.Animate(
                0.5, // duration
                () => { Alpha = 0; },
                () => { RemoveFromSuperview(); }
            );
        }
    }
}

