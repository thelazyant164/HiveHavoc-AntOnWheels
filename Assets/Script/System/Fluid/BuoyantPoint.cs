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

        public float GetSubmergedVolume() =>
            Mathf.Clamp01(Fluid.SampleSurfaceHeight(transform.position) - transform.position.y ?? 0)
            * Volume
            * WeightDistribution;

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
