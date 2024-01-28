using GalaxyQuest.Entities;

namespace GalaxyQuest.Stuff.Weapons.OffensiveWeapons
{
    /// <summary>
    /// Represents a cannon weapon, a type of offensive weapon.
    /// </summary>
    public class CanonWeapon : OffensiveWeapon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanonWeapon"/> class.
        /// </summary>
        public CanonWeapon() : base(WeaponName.Canon.ToString().ToLower(), WeaponType.Offensive.ToString().ToLower(), 10)
        {
        }
    }
}