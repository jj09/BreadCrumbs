using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreadCrumbs.Shared.Models;
using Plugin.Geolocator;
using BreadCrumbs.Shared.Helpers;

namespace BreadCrumbs.Shared.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Place> SavedPlaces;

        public MainViewModel()
        {
            SavedPlaces = new ObservableCollection<Place>();
        }

        async public Task<bool> SaveAsync(string name)
        {
            var coordinates = await LocationHelper.GetCurrentLocation();

            SavedPlaces.Add(new Place(name, coordinates.Lat, coordinates.Long));

            return true;
        }
    }
}
