namespace GalaxyQuest.Entities
{
    public class StarSystem
    {
        public string Name { get; private set; }
        public List<Planet> Planets { get; private set; }
        public StarSystem(string name, List<Planet> planets) => (Name, Planets) = (name, planets);
    }
}
