using GalaxyQuest.Entities;

namespace GalaxyQuest.Stuff.Weapons.OffensiveWeapons
{
    /// <summary>
    /// Represents a bomb bay weapon, a type of offensive weapon.
    /// </summary>
    public class BombBayWeapon : OffensiveWeapon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BombBayWeapon"/> class.
        /// </summary>
        public BombBayWeapon() : base(WeaponName.BombBay.ToString().ToLower(), WeaponType.Offensive.ToString().ToLower(), 400)
        {
        }
    }
}