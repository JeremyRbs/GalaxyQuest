using GalaxyQuest.Entities;
using GalaxyQuest.Stuff.Weapons.DefensiveWeapons;
using GalaxyQuest.Stuff.Weapons.OffensiveWeapons;

namespace GalaxyQuest.Units.CombatUnits
{
    /// <summary>
    /// Represents a bomber combat unit.
    /// </summary>
    public class BomberUnit : CombatUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BomberUnit"/> class.
        /// </summary>
        /// <param name="system">The star system where the unit is located.</param>
        /// <param name="planet">The planet where the unit is located.</param>
        /// <param name="userId">The identifier of the user to whom the unit belongs.</param>
        public BomberUnit(string system, string planet, string userId)
            : base(UnitType.Bomber.ToString().ToLower(), system, planet, userId, 50, new List<Weapon> { new BombBayWeapon(), new DeflectorWeapon() }, new List<string> { UnitType.Cruiser.ToString().ToLower(), UnitType.Bomber.ToString().ToLower(), UnitType.Fighter.ToString().ToLower() }, 60)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BomberUnit"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the unit.</param>
        /// <param name="system">The star system where the unit is located.</param>
        /// <param name="planet">The planet where the unit is located.</param>
        /// <param name="destinationSystem">The destination star system for the unit.</param>
        /// <param name="destinationPlanet">The destination planet for the unit.</param>
        /// <param name="userId">The identifier of the user to whom the unit belongs.</param>
        public BomberUnit(string id, string system, string planet, string destinationSystem, string destinationPlanet, string userId)
            : base(id, UnitType.Bomber.ToString().ToLower(), system, planet, destinationSystem, destinationPlanet, userId, 50, new List<Weapon> { new BombBayWeapon(), new DeflectorWeapon() }, new List<string> { UnitType.Cruiser.ToString().ToLower(), UnitType.Bomber.ToString().ToLower(), UnitType.Fighter.ToString().ToLower() }, 60)
        {
        }
    }
}
