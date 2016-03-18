using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace BreadCrumbs.Shared.Models
{
    public class Place
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey(typeof(Coordinates))]
        public int CoordinatesId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Coordinates Coordinates { get; set; }

        [Ignore]
        public string DisplayName
        {
            get
            {
                if (Coordinates != null)
                {
                    return Name + $" ({Coordinates.Lat.ToString("#.##")}, {Coordinates.Long.ToString("#.##")})";
                }
                else
                {
                    return Name;
                }
            }
        }

        public Place()
        {

        }

        public Place(string name, double lat, double lng)
        {
            Coordinates = new Coordinates(lat, lng);
            Name = name;
        }
    }
}