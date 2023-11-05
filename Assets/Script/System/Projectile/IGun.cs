using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile
{
    public interface IGun
    {
        public IDepletableAmmo Ammo { get; }
    }
}