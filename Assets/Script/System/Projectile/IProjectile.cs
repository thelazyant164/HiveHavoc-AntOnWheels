using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile
{
    public interface IProjectile : IDamaging, IDestructible
    {
        public LayerMask InterceptedBy { get; }

        public void Launch(Vector3 spatialImpulse);
    }
}
