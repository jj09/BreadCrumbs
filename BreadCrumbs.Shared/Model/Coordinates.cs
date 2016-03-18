namespace BreadCrumbs.Shared.Models
{
    public class Coordinates
    {
        public Coordinates(double lat, double lng)
        {
            Lat = lat;
            Long = lng;
        }

        public double Lat { get; set; }

        public double Long { get; set; }

    }
}