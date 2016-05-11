using BreadCrumbs.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BreadCrumbs.Tests.Models
{
    public class CoordinatesTests
    {
        [Fact]
        public void Constructor_Sets_Coordinates()
        {
            // Arrange
            var latitude = 50.45;
            var longitude = 31.32;

            // Act
            var coordinates = new Coordinates(latitude, longitude);

            // Assert
            Assert.Equal(latitude, coordinates.Lat);
            Assert.Equal(longitude, coordinates.Long);
        }
    }
}
