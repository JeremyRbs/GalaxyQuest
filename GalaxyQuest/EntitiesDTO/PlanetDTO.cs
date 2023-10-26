namespace GalaxyQuest.EntitiesDTO
{
    public class PlanetDTO
    {
        public string Name { get; }
        public int Size { get; }
        public Dictionary<string, int>? ResourceQuantity { get; }
        public PlanetDTO(string name, int size, Dictionary<string, int>? resourceQuantity) => (Name, Size, ResourceQuantity) = (name, size, resourceQuantity);
    }
}
