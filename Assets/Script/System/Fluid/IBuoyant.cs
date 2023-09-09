using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    public interface IBuoyant : IDynamic
    {
        public abstract Rigidbody Rigidbody { get; }
        public abstract float Volume { get; }
        public abstract float Density { get; }
        public abstract float RatioToBoundVolume { get; }
        public abstract float SubmergedDimensionDepth { get; }

        public abstract void ResetMass();
    }
}
