using Bitgem.VFX.StylisedWater;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public sealed class BuoyantPlatform : MonoBehaviour, IFloatable, IMovable
    {
        public Rigidbody Rigidbody { get; private set; }
        private Collider floatingCollider;
        public IFluidBody Fluid { get; private set; }

        [SerializeField]
        private float density;
        public float Density => density;

        public float Volume { get; private set; }

        [SerializeField]
        private float ratioToBoundVolume = 1;
        public float RatioToBoundVolume => ratioToBoundVolume;

        [SerializeField]
        private float explosionUpwardForceModifier;
        public float ExplosionUpwardForceModifier => explosionUpwardForceModifier;

        private float offsetY;

        private void Awake()
        {
            ResetMass();
        }

        private void FixedUpdate()
        {
            if (Fluid == null)
                return;
            Rigidbody.AddForce(0, GetBuoyancyForce(), 0);
            Rigidbody.AddForce(GetBuoyancyDrag(), ForceMode.VelocityChange);
            Rigidbody.AddTorque(GetBuoyancyAngularDrag(), ForceMode.VelocityChange);
        }

        public void ResetMass()
        {
            Rigidbody = GetComponent<Rigidbody>();
            floatingCollider = GetComponent<Collider>();
            Volume = floatingCollider.GetBoundVolume() * ratioToBoundVolume;
            offsetY = floatingCollider.bounds.size.y / 2;
            Rigidbody.mass = density * Volume;
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
            Mathf.Clamp01(
                Fluid.SampleSurfaceHeight(Rigidbody.position) - (Rigidbody.position.y - offsetY)
                    ?? 0
            ) * Volume;

        public float GetBuoyancyForce() =>
            Mathf.Abs(Physics.gravity.y) * Fluid.Density * GetSubmergedVolume();

        public Vector3 GetBuoyancyDrag() =>
            GetSubmergedVolume() * (-Rigidbody.velocity) * Fluid.Drag * Time.fixedDeltaTime;

        public Vector3 GetBuoyancyAngularDrag() =>
            GetSubmergedVolume()
            * (-Rigidbody.angularVelocity)
            * Fluid.AngularDrag
            * Time.fixedDeltaTime;

        public void ReactTo<T>(Explosion<T> explosion) =>
            Rigidbody.AddExplosionForce(
                explosion.force,
                explosion.epicenter,
                explosion.radius,
                explosionUpwardForceModifier,
                ForceMode.Force
            );
    }
}
