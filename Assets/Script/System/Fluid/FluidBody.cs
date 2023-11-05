using Bitgem.VFX.StylisedWater;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    [RequireComponent(typeof(WaterVolumeBox), typeof(WaterVolumeHelper), typeof(BoxCollider))]
    public sealed class FluidBody : MonoBehaviour, IFluidBody
    {
        private WaterVolumeHelper helper;
        public float Density { get; private set; }
        public float Drag { get; private set; }
        public float AngularDrag { get; private set; }

        private void Awake()
        {
            RegisterFluidVolume();
            gameObject.TryFindImmediateComponent(out helper);
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
            if (collider.gameObject.TryFindImmediateComponent(out IProjectile projectile))
            {
                projectile.Destroy();
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

        public void RegisterFluidVolume()
        {
            WaterVolumeBox fluidBody = GetComponent<WaterVolumeBox>();
            WaterVolumeHelper fluidHelper = GetComponent<WaterVolumeHelper>();
            BoxCollider fluidVolume = GetComponent<BoxCollider>();

            fluidHelper.WaterVolume = fluidBody;

            fluidVolume.isTrigger = true;
            fluidVolume.size = new Vector3(
                fluidBody.Dimensions.x,
                fluidBody.Dimensions.y * fluidBody.TileSize,
                fluidBody.Dimensions.z
            );
            fluidVolume.center = (fluidVolume.size - Vector3.one) / 2;
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
