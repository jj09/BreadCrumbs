using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BreadCrumbs.Shared.Helpers;
using BreadCrumbs.Shared.Models;
using BreadCrumbs.Shared.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BreadCrumbs.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel;

        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new MainViewModel();
            savedPlacesListView.ItemsSource = ViewModel.SavedPlaces;
        }

        // saveButton_Click and nameTextBox_KeyUp can be combined into 1 method 
        // (as KeyRoutedEventArgs inherit from RoutedEventArgs) - NOT EFFICIENT THOUGH
        //var keyREA = e as KeyRoutedEventArgs;
        //if ( (keyREA != null && keyREA.Key == Windows.System.VirtualKey.Enter)
        //    || keyREA == null)
        //{
        //    SavePlaceAndClearTextBox();
        //}

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SavePlaceAndClearTextBox();
        }

        private void nameTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SavePlaceAndClearTextBox();
            }
        }

        private void SavePlaceAndClearTextBox()
        {
            ViewModel.SaveAsync(this.nameTextBox.Text);

            this.nameTextBox.Text = "";
        }

        private void savedPlacesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // API: https://msdn.microsoft.com/en-us/windows/uwp/launch-resume/launch-maps-app?f=255&MSPPError=-2147217396

            var selectedPlace = e.ClickedItem as Place;
            var selectedPlaceCoordinates = selectedPlace.Coordinates;

            // ms-walk-to api (doesn't need current location as param)
            Windows.System.Launcher.LaunchUriAsync(new Uri($"ms-walk-to:?destination.latitude={selectedPlaceCoordinates.Lat}&destination.longitude={selectedPlaceCoordinates.Long}"));

            //// bingmaps api
            //var currentLocationCoordinates = await LocationHelper.GetCurrentLocation();
            //await Windows.System.Launcher.LaunchUriAsync(new Uri($"bingmaps:?rtp=pos.{currentLocationCoordinates.Lat}_{currentLocationCoordinates.Long}~pos.{selectedPlace.Coordinates.Lat}_{selectedPlace.Coordinates.Long}&mode=w"));
        }
    }
}
