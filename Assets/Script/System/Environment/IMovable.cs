using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public interface IMovable : IDynamic
    {
        public abstract float ExplosionUpwardForceModifier { get; }

        public void ReactTo<T>(Explosion<T> explosion);
        public void ResetTo(Vector3 position, Quaternion rotation);
    }
}
