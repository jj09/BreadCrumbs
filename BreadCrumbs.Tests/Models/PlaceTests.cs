using BreadCrumbs.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BreadCrumbs.Tests.Models
{
    public class PlaceTests
    {
        [Fact]
        public void Creating_Place_With_Constructor()
        {
            // Arrange
            var name = "fake place";
            var latitude = 50.45;
            var longitude = 31.32;
            var displayName = name + $" ({latitude.ToString("#.##")}, {longitude.ToString("#.##")})";

            // Act
            var place = new Place(name, latitude, longitude);

            // Assert
            Assert.Equal(latitude, place.Coordinates.Lat);
            Assert.Equal(longitude, place.Coordinates.Long);
            Assert.Equal(name, place.Name);
            Assert.Equal(displayName, place.DisplayName);
        }
    }
}
