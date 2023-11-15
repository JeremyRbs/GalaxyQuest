namespace GalaxyQuest.Entities
{
    public class Unit
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string System { get; set; }
        public string? Planet { get; set; }
        public Unit(string type, string system) => (Id, Type, System, Planet) = (Guid.NewGuid().ToString(), type, system, null);
        public void MoveUnit(string systemName, string planetName) => (System, Planet) = (systemName, planetName);
    }
}
