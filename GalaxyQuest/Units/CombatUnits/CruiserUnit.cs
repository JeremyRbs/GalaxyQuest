using GalaxyQuest.Entities;
using GalaxyQuest.Stuff.Weapons;
using GalaxyQuest.Stuff.Weapons.OffensiveWeapons;
using System.Collections.Generic;

namespace GalaxyQuest.Units.CombatUnits
{
    /// <summary>
    /// Represents a cruiser combat unit.
    /// </summary>
    public class CruiserUnit : CombatUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CruiserUnit"/> class.
        /// </summary>
        /// <param name="system">The star system where the unit is located.</param>
        /// <param name="planet">The planet where the unit is located.</param>
        /// <param name="userId">The identifier of the user to whom the unit belongs.</param>
        public CruiserUnit(string system, string planet, string userId)
            : base(UnitType.Cruiser.ToString().ToLower(), system, planet, userId, 400, new List<Weapon> { new CanonWeapon(), new CanonWeapon(), new CanonWeapon(), new CanonWeapon() }, new List<string> { UnitType.Fighter.ToString().ToLower(), UnitType.Cruiser.ToString().ToLower(), UnitType.Bomber.ToString().ToLower() }, 6)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CruiserUnit"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the unit.</param>
        /// <param name="system">The star system where the unit is located.</param>
        /// <param name="planet">The planet where the unit is located.</param>
        /// <param name="destinationSystem">The destination star system for the unit.</param>
        /// <param name="destinationPlanet">The destination planet for the unit.</param>
        /// <param name="userId">The identifier of the user to whom the unit belongs.</param>
        public CruiserUnit(string id, string system, string planet, string destinationSystem, string destinationPlanet, string userId)
            : base(id, UnitType.Cruiser.ToString().ToLower(), system, planet, destinationSystem, destinationPlanet, userId, 400, new List<Weapon> { new CanonWeapon(), new CanonWeapon(), new CanonWeapon(), new CanonWeapon() }, new List<string> { UnitType.Fighter.ToString().ToLower(), UnitType.Cruiser.ToString().ToLower(), UnitType.Bomber.ToString().ToLower() }, 6)
        {
        }
    }
}
