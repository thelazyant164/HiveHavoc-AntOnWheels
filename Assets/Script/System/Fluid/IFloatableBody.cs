using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    public interface IFloatableBody : IBuoyant
    {
        public abstract int FloatPoints { get; }

        public abstract void RegisterFloatPoints();
    }
}
