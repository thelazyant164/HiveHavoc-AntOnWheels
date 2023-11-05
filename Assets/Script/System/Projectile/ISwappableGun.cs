using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile
{
    public abstract class AmmoDictionary : SerializableDictionary<Ammo, ISwappableShooter> { }

    public interface ISwappableGun : IGun
    {
        public abstract AmmoDictionary AmmoType { get; }

        public abstract void OnShoot(object sender, Ammo ammo);
    }
}
