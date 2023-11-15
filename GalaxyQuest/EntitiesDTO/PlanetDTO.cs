namespace GalaxyQuest.EntitiesDTO
{
    public class PlanetDTO
    {
        public string Name { get; private set; }
        public int Size { get; private set; }
        public Dictionary<string, int>? ResourceQuantity { get; private set; }
        public PlanetDTO(string name, int size, Dictionary<string, int>? resourceQuantity) => (Name, Size, ResourceQuantity) = (name, size, resourceQuantity);
    }
}
