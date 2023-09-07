using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    public interface IFloatable : IBuoyant
    {
        public abstract IFluidBody Fluid { get; }

        public abstract void Enter(IFluidBody fluid);
        public abstract void Exit(IFluidBody fluid);
        public abstract float GetSubmergedVolume();
        public abstract float GetBuoyancyForce();
        public abstract Vector3 GetBuoyancyDrag();
        public abstract Vector3 GetBuoyancyAngularDrag();
    }
}
