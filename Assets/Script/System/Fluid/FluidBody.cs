using Bitgem.VFX.StylisedWater;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    public sealed class FluidBody : MonoBehaviour, IFluidBody
    {
        private WaterVolumeHelper helper;
        public float Density {get; private set;}
        public float Drag { get; private set; }
        public float AngularDrag { get; private set; }

        private void Start()
        {
            helper = GetComponentInParent<WaterVolumeHelper>();
            if (helper == null)
                Debug.LogError("Water helper not detected");
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.TryFindImmediateComponent(out IFloatable floatable))
            {
                //Debug.LogWarning($"{collider.gameObject} begin touching water");
                floatable.Enter(this);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.TryFindImmediateComponent(out IFloatable floatable))
            {
                //Debug.LogWarning($"{collider.gameObject} stop touching water");
                floatable.Exit(this);
            }
        }

        public float? SampleSurfaceHeight(Vector3 position) => helper.GetHeight(position);

        public void RegisterTo(IFluid fluid)
        {
            Density = fluid.Density;
            Drag = fluid.Drag;
            AngularDrag = fluid.AngularDrag;
        }
    }
}
