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
        public Rigidbody Rigidbody { get; private set; }
        public IFluidBody Fluid { get; private set; }
        public float Volume { get; private set; }
        public float Density { get; private set; }
        public float RatioToBoundVolume { get; private set; }
        public float SubmergedDimensionDepth { get; private set; }
        public float WeightDistribution { get; private set; }
        public float ExplosionUpwardForceModifier => 0;

        private void Start()
        {
            ResetMass();
        }

        private void FixedUpdate()
        {
            if (Rigidbody == null || Fluid == null || WeightDistribution == 0)
                return;
            Vector3 buoyancyForceAtPoint = new Vector3(0, GetBuoyancyForce(), 0);
            Rigidbody.AddForceAtPosition(buoyancyForceAtPoint, transform.position);
            Rigidbody.AddForce(GetBuoyancyDrag(), ForceMode.VelocityChange);
            Rigidbody.AddTorque(GetBuoyancyAngularDrag(), ForceMode.VelocityChange);
        }

        public void ResetMass()
        {
            body = GetComponentInParent<IFloatableBody>();

            Volume = body.Volume;
            Density = body.Density;
            RatioToBoundVolume = body.RatioToBoundVolume;
            SubmergedDimensionDepth = body.SubmergedDimensionDepth;
            Rigidbody = body.Rigidbody;
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

        public void ReactTo<T>(Explosion<T> explosion) { }
    }
}
