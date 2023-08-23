using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile
{
    public interface IDepletableAmmo
    {
        public abstract int MaxAmmo { get; }
        public abstract int Ammo { get; }
        public abstract int MaxAmmoStock { get; }
        public abstract int AmmoStock { get; }

        public abstract void Consume(int ammo);
        public abstract void Restock();
        public abstract void Reload();
    }
}
