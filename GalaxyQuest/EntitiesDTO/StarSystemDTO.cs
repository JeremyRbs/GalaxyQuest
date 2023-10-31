namespace GalaxyQuest.EntitiesDTO
{

    public class StarSystemDTO
    {
<<<<<<< HEAD
        public string Name { get; private set; }
        public List<PlanetDTO> Planets { get; private set; }
=======
        public string Name { get; }
        public List<PlanetDTO> Planets { get; }
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
        public StarSystemDTO(string name, List<PlanetDTO> planets) => (Name, Planets) = (name, planets);
    }

}
