using BreadCrumbs.Shared.Models;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadCrumbs.Shared.Helpers
{
    public class LocationHelper
    {
        async public static Task<Coordinates> GetCurrentLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 10;
            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            var coordinates = new Coordinates(position.Latitude, position.Longitude);

            return coordinates;
        }
    }
}
