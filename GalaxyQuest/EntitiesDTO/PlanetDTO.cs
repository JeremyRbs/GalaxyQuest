namespace GalaxyQuest.EntitiesDTO
{
    public class PlanetDTO
    {
<<<<<<< HEAD
        public string Name { get; private set; }
        public int Size { get; private set; }
        public Dictionary<string, int>? ResourceQuantity { get; private set; }
=======
        public string Name { get; }
        public int Size { get; }
        public Dictionary<string, int>? ResourceQuantity { get; }
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        public PlanetDTO(string name, int size, Dictionary<string, int>? resourceQuantity) => (Name, Size, ResourceQuantity) = (name, size, resourceQuantity);
    }
}
