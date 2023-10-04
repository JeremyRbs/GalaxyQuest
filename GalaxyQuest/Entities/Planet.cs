namespace GalaxyQuest.Entities
{
    public class Planet
    {
        public string Name { get; private set; }
        public int Size { get; private set; }
        public Planet(string name, int size) => (Name, Size) = (name, size);
    }
}
