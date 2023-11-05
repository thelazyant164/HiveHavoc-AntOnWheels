using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile
{
    public abstract class ISwappableShooter : MonoBehaviour // due to Unity's limitation of not serializing Interface
    {
        public abstract int AmmoCost { get; }

        public abstract void Shoot();
    }
}
