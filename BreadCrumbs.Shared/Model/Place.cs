namespace BreadCrumbs.Shared.Models
{
    public class Place
    {
        public string Name { get; set; }
        public Coordinates Coordinates { get; set; }

        public Place(string name, double lat, double lng)
        {
            Coordinates = new Coordinates(lat, lng);
            Name = name;
        }
    }
}