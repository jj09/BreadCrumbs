using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace BreadCrumbs.Shared.Models
{
    public class Place
{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreateDateTime { get; set; }

        [ForeignKey(typeof(Coordinates))]
        public int CoordinatesId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert | CascadeOperation.CascadeDelete)]
        public Coordinates Coordinates { get; set; }

        [Ignore]
        public string DisplayName => Coordinates != null ? Name + $" ({CoordinatesString})" : Name;

        [Ignore]
        public string DateAndCoordinates => $"{CoordinatesString}  /  {CreateDateTime.ToLocalTime().ToString("yyyy-MM-dd hh:mm tt")}";

        [Ignore]
        private string CoordinatesString => $"{Coordinates?.Lat.ToString("#.##")}, {Coordinates?.Long.ToString("#.##")}";

        public Place()
        {

        }

        public Place(string name, double lat, double lng)
        {
            Coordinates = new Coordinates(lat, lng);
            Name = name;
            CreateDateTime = DateTime.UtcNow;
        }
    }
}