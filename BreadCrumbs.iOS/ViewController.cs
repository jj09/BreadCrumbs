﻿using System;

using UIKit;
using BreadCrumbs.Shared.ViewModels;
using System.Linq;
using Foundation;
using BreadCrumbs.Shared.Models;
using BreadCrumbs.Shared.Helpers;

namespace BreadCrumbs.iOS
{
	public partial class ViewController : UIViewController
	{
		MainViewModel ViewModel;

		public ViewController (IntPtr handle) : base (handle)
		{
			ViewModel = new MainViewModel();

			//this.saveButton.TouchUpInside += (object sender, EventArgs e) => 
			//{
			//	var name = this.placeNameTextField.Text;
			//	ViewModel.SaveAsync(name);
   //             this.placeNameTextField.Text = "";
   //         };
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
            PlacesTableView.ReloadData();
        }

        async partial void SaveClick(UIButton sender)
        {
            var name = this.placeNameTextField.Text;
            await ViewModel.SaveAsync(name);
            PlacesTableView.ReloadData();
            this.placeNameTextField.Text = "";
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
	}
}

