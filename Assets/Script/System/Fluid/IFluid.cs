using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    public interface IFluid
    {
        public abstract float Density { get; }
        public abstract float Drag { get; }
        public abstract float AngularDrag { get; }
    }
}
