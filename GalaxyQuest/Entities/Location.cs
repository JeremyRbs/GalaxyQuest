namespace GalaxyQuest.Entities
{
    public class Location
    {
        public string System { get; set; }
        public string Planet { get; set; }
        public Dictionary<string, int>? ResourcesQuantity { get; set; }
        public Location(string system, string planet, Dictionary<string, int>? resourcesQuantity) => (System, Planet, ResourcesQuantity) = (system, planet, resourcesQuantity);
    }

}
