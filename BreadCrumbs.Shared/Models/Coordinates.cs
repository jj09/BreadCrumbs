using SQLite.Net.Attributes;

namespace BreadCrumbs.Shared.Models
{
    public class Coordinates
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public double Lat { get; set; }

        public double Long { get; set; }

        public Coordinates()
        {

        }

        public Coordinates(double lat, double lng)
        {
            Lat = lat;
            Long = lng;
        }
    }
}