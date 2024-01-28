namespace GalaxyQuest.Units.PeacefulUnits
{
    /// <summary>
    /// Represents a builder unit in the galaxy.
    /// </summary>
    public class BuilderUnit : PeacefulUnit
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BuilderUnit"/> class.
        /// </summary>
        public BuilderUnit(string system, string planet, string userId)
            : base(UnitType.Builder.ToString().ToLower(), system, planet, userId)
        { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BuilderUnit"/> class.
        /// </summary>
        public BuilderUnit(string id, string system, string planet, string destinationSystem, string destinationPlanet, string userId)
            : base(id, UnitType.Builder.ToString().ToLower(), system, planet, destinationSystem, destinationPlanet, userId)
        { }
        
    }
}
