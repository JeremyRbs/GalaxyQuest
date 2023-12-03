namespace GalaxyQuest.EntitiesDTO
{

    public class StarSystemDTO
    {
        public string Name { get; }
        public List<PlanetDTO> Planets { get; }
        public StarSystemDTO(string name, List<PlanetDTO> planets) => (Name, Planets) = (name, planets);
    }

}
