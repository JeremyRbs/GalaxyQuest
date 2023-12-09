namespace GalaxyQuest.Units.PeacefulUnits
{
    /// <summary>
    /// Represents a scout unit in the galaxy.
    /// </summary>
    public class ScoutUnit : PeacefulUnit
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoutUnit"/> class.
        /// </summary>
        public ScoutUnit(string system, string planet, string userId)
            : base(UnitType.Scout.ToString().ToLower(), system, planet, userId)
        {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoutUnit"/> class.
        /// </summary>
        public ScoutUnit(string id, string system, string planet, string destinationSystem, string destinationPlanet, string userId)
            : base(id, UnitType.Scout.ToString().ToLower(), system, planet, destinationSystem, destinationPlanet, userId)
        { }
    }
}
