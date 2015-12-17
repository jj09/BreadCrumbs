namespace BreadCrumbs.Shared.Models
{
    public class Place
    {
        public string Name { get; set; }
        public Coordinates Coordinates { get; set; }

        public Place(string name, float lat, float lng)
        {
            Coordinates = new Coordinates
            {
                Lat = lat,
                Long = lng
            };
            Name = name;
        }
    }
}