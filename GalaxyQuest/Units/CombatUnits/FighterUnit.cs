using GalaxyQuest.Entities;
using GalaxyQuest.Stuff.Weapons;
using GalaxyQuest.Stuff.Weapons.OffensiveWeapons;
using System.Collections.Generic;

namespace GalaxyQuest.Units.CombatUnits
{
    /// <summary>
    /// Represents a fighter combat unit.
    /// </summary>
    public class FighterUnit : CombatUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FighterUnit"/> class.
        /// </summary>
        /// <param name="system">The star system where the unit is located.</param>
        /// <param name="planet">The planet where the unit is located.</param>
        /// <param name="userId">The identifier of the user to whom the unit belongs.</param>
        public FighterUnit(string system, string planet, string userId)
            : base(UnitType.Fighter.ToString().ToLower(), system, planet, userId, 80, new List<Weapon> { new CanonWeapon() }, new List<string> { UnitType.Bomber.ToString().ToLower(), UnitType.Fighter.ToString().ToLower(), UnitType.Cruiser.ToString().ToLower() }, 6)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FighterUnit"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the unit.</param>
        /// <param name="system">The star system where the unit is located.</param>
        /// <param name="planet">The planet where the unit is located.</param>
        /// <param name="destinationSystem">The destination star system for the unit.</param>
        /// <param name="destinationPlanet">The destination planet for the unit.</param>
        /// <param name="userId">The identifier of the user to whom the unit belongs.</param>
        public FighterUnit(string id, string system, string planet, string destinationSystem, string destinationPlanet, string userId)
            : base(id, UnitType.Fighter.ToString().ToLower(), system, planet, destinationSystem, destinationPlanet, userId, 80, new List<Weapon> { new CanonWeapon() }, new List<string> { UnitType.Bomber.ToString().ToLower(), UnitType.Fighter.ToString().ToLower(), UnitType.Cruiser.ToString().ToLower() }, 6)
        {
        }
    }
}
