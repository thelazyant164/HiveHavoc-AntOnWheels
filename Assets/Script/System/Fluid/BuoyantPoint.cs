using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    [RequireComponent(typeof(Collider))]
    public sealed class BuoyantPoint : MonoBehaviour, IFloatable
    {
        private IFloatableBody body;
        public Rigidbody Rigidbody => body.Rigidbody;
        public IFluidBody Fluid { get; private set; }
        public float Volume => body.Volume;
        public float Density => body.Density;
        public float RatioToBoundVolume => body.RatioToBoundVolume;
        public float SubmergedDimensionDepth => body.SubmergedDimensionDepth;
        public float WeightDistribution
        {
            get => weightDistribution;
            private set => weightDistribution = value;
        }
        public float ExplosionUpwardForceModifier => 0;

        [SerializeField]
        private float buoyantForce;

        [SerializeField]
        private float weightDistribution;

        private void Start()
        {
            ResetMass();
        }

        private void FixedUpdate()
        {
            if (Rigidbody == null || Fluid == null || WeightDistribution == 0)
                return;
            buoyantForce = GetBuoyancyForce();
            Vector3 buoyancyForceAtPoint = new Vector3(0, buoyantForce, 0);
            Rigidbody.AddForceAtPosition(buoyancyForceAtPoint, transform.position);
            Rigidbody.AddForce(GetBuoyancyDrag(), ForceMode.VelocityChange);
            Rigidbody.AddTorque(GetBuoyancyAngularDrag(), ForceMode.VelocityChange);
        }

        public void ResetMass()
        {
            body = GetComponentInParent<IFloatableBody>();
            WeightDistribution = (float)1 / body.FloatPoints;
        }

        public void Enter(IFluidBody fluid)
        {
            Fluid = fluid;
        }

        public void Exit(IFluidBody fluid)
        {
            Fluid = null;
        }

        public float GetSubmergedVolume()
        {
            float surfaceHeight = Fluid.SampleSurfaceHeight(transform.position) ?? 0;
            // TODO: issue: surface height keeps increasing - due to honey material config, adjust this
            //Debug.Log($"Surface height is {surfaceHeight}");
            float worldSpaceY = transform.position.y;
            //Debug.Log($"World space Y is {worldSpaceY}");
            float submergedDepth = surfaceHeight - worldSpaceY;
            //Debug.Log($"Submerged depth is {submergedDepth}");
            float unclampedSubmergedRatio = submergedDepth / SubmergedDimensionDepth;
            //Debug.Log($"Unclamped submerged ratio is {unclampedSubmergedRatio}");
            float clampedSubmergedRatio = Mathf.Clamp01(unclampedSubmergedRatio);
            //Debug.Log($"Clamped submerged ratio is {clampedSubmergedRatio}");
            float roughSubmergedVolume = clampedSubmergedRatio * Volume * WeightDistribution;
            //Debug.Log($"Submerged volume is {roughSubmergedVolume}");
            return roughSubmergedVolume;
        }

        public float GetBuoyancyForce() =>
            Mathf.Abs(Physics.gravity.y) * Fluid.Density * GetSubmergedVolume();

        public Vector3 GetBuoyancyDrag() =>
            GetSubmergedVolume() * (-Rigidbody.velocity) * Fluid.Drag * Time.fixedDeltaTime;

        public Vector3 GetBuoyancyAngularDrag() =>
            GetSubmergedVolume()
            * (-Rigidbody.angularVelocity)
            * Fluid.AngularDrag
            * Time.fixedDeltaTime;
    }
}
