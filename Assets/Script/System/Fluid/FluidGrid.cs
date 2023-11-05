using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    public sealed class FluidGrid : MonoBehaviour, IFluid
    {
        [SerializeField]
        private float density;
        public float Density => density;

        [SerializeField]
        private float drag;
        public float Drag => drag;

        [SerializeField]
        private float angularDrag;
        public float AngularDrag => angularDrag;

        private void Awake()
        {
            IEnumerable<IFluidBody> fluids = GetComponentsInChildren<IFluidBody>();
            foreach (IFluidBody body in fluids) body.RegisterTo(this);
        }
    }
}
