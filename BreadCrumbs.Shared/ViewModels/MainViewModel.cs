using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreadCrumbs.Shared.Models;
using BreadCrumbs.Shared.Helpers;
using System.IO;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLiteNetExtensions.Extensions;

namespace BreadCrumbs.Shared.ViewModels
{
    public class MainViewModel
    {
        // This class uses https://bitbucket.org/twincoders/sqlite-net-extensions

        private static string _dbFileName = "BreadCrumbs_v_1_0.db3";

        public ObservableCollection<Place> SavedPlaces;
        private object _locker = new object(); // SQLite Db locker
        private string _dbPath;
        private ISQLitePlatform _platform;

        public MainViewModel()
        {
#if __ANDROID__
            // Just use whatever directory SpecialFolder.Personal returns
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            _platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
#elif __IOS__
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder instead
            _platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
#else
            // UWP (TODO: change to #elif for case when more platforms will be supported - web?)
            var libraryPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            _platform = new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT();
#endif
            _dbPath = Path.Combine(libraryPath, _dbFileName);

            using (var conn = GetSQLiteConn())
            {
                conn.CreateTable<Coordinates>();
                conn.CreateTable<Place>();

                var places = conn.GetAllWithChildren<Place>().ToList();

                SavedPlaces = new ObservableCollection<Place>(places);
            }
        }

        private SQLiteConnection GetSQLiteConn() => new SQLiteConnection(_platform, _dbPath, true, null);

        async public Task<bool> SaveAsync(string name)
        {
            var coordinates = await LocationHelper.GetCurrentLocation();

            var place = new Place(name, coordinates.Lat, coordinates.Long);

            SavedPlaces.Add(place);

            using (var conn = GetSQLiteConn())
            {
                lock (_locker)
                {
                    conn.InsertWithChildren(place);                    
                    conn.Commit();
                }
            }

            return true;
        }

        public void Remove(Place place)
        {
            SavedPlaces.Remove(place);

            using (var conn = GetSQLiteConn())
            {
                lock (_locker)
                {
                    conn.Delete<Place>(place.Id);
                    conn.Delete<Coordinates>(place.CoordinatesId);
                    conn.Commit();
                }
            }
        }
    }
}
