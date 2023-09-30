using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile
{
    public interface IDepletableAmmo
    {
        public int MaxAmmo { get; }
        public int Ammo { get; }
        public int MaxAmmoStock { get; }
        public int AmmoStock { get; }

        public void Consume(int ammo);
        public void Restock();
        public void Reload();
    }
}
