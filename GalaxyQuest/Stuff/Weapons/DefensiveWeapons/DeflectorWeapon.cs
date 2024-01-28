using GalaxyQuest.Entities;

namespace GalaxyQuest.Stuff.Weapons.DefensiveWeapons
{
    /// <summary>
    /// Represents a deflector weapon, a type of defensive weapon.
    /// </summary>
    public class DeflectorWeapon : DefensiveWeapon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeflectorWeapon"/> class.
        /// </summary>
        public DeflectorWeapon() : base(WeaponName.Deflector.ToString().ToLower(), WeaponType.Defensive.ToString().ToLower(), 10)
        {
        }
    }
}