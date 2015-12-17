using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreadCrumbs.Shared.Models;

namespace BreadCrumbs.Shared.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Place> SavedPlaces;

        public MainViewModel()
        {
            SavedPlaces = new ObservableCollection<Place>();
        }

        public void Save(string name)
        {
            SavedPlaces.Add(new Place(name, 56, -43));
        }
    }
}
