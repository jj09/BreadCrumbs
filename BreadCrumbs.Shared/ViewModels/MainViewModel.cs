using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreadCrumbs.Shared.Models;
using Plugin.Geolocator;

namespace BreadCrumbs.Shared.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Place> SavedPlaces;

        public MainViewModel()
        {
            SavedPlaces = new ObservableCollection<Place>();
        }

        public void SaveAsync(string name, Coordinates coordinates)
        {
            //var locator = CrossGeolocator.Current;
            //locator.DesiredAccuracy = 50;
            //var position = await locator.GetPositionAsync();
            //SavedPlaces.Add(new Place(name, position.Latitude, position.Longitude));

            SavedPlaces.Add(new Place(name, coordinates.Lat, coordinates.Long));
        }
    }
}
