using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat
{
    public interface IProjectile : IDamaging
    {
        public LayerMask Blocking { get; }

        public void Launch(Vector3 spatialImpulse);
    }
}
