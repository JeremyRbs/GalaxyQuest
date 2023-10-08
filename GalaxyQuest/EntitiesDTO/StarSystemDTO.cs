namespace GalaxyQuest.EntitiesDTO
{

    public class StarSystemDTO
    {
        public string Name { get; private set; }
        public List<PlanetDTO> Planets { get; private set; }
        public StarSystemDTO(string name, List<PlanetDTO> planets) => (Name, Planets) = (name, planets);
    }

}
