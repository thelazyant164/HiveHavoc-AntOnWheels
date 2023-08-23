using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public interface IMovable : IDynamic
    {
        public abstract float UpwardForce { get; }

        public void ReactTo<T>(Explosion<T> explosion);
    }
}
