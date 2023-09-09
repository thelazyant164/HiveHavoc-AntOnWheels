using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    public interface IFluidBody : IFluid
    {
        public abstract void RegisterFluidVolume();
        public abstract void RegisterTo(IFluid fluid);
        public abstract float? SampleSurfaceHeight(Vector3 position);
    }
}
