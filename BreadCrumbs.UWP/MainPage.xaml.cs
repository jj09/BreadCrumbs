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

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var currentLocationCoordinates = getCoordinates().Result;

            ViewModel.SaveAsync(this.nameTextBox.Text, currentLocationCoordinates);
        }
        
        private async void SavedPlacesListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentLocationCoordinates = getCoordinates().Result;

            var selectedPlace = ((ListView)sender).SelectedItem as Place;
            await Windows.System.Launcher.LaunchUriAsync(new Uri($"bingmaps:?rtp=pos.{currentLocationCoordinates.Lat}_{currentLocationCoordinates.Long}~pos.{selectedPlace.Coordinates.Lat}_{selectedPlace.Coordinates.Long}"));
        }

        private async Task<Coordinates> getCoordinates()
        {
            try
            {
                Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 10, MovementThreshold = 5, ReportInterval = 5};

                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                         maximumAge: TimeSpan.FromMinutes(5),
                         timeout: TimeSpan.FromSeconds(10)
                        );

                var currentLat = geoposition.Coordinate.Latitude;
                var currentLong = geoposition.Coordinate.Longitude;

                return new Coordinates(currentLat, currentLong);
            }
            catch (Exception e)
            {
                return new Coordinates(0, 0);
            }
            
        }
    }
}
