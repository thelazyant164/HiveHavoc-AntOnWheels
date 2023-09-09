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

        [Header("Buoyancy config")]
        [SerializeField]
        private float density;
        public float Density => density;

        public float Volume { get; private set; }

        [SerializeField]
        private float ratioToBoundVolume = 1;
        public float RatioToBoundVolume => ratioToBoundVolume;
        [SerializeField]
        private float submergedDimensionDepth;
        public float SubmergedDimensionDepth => submergedDimensionDepth;

        [Space]

        [Header("Dynamic environment config")]
        [SerializeField]
        private float explosionUpwardForceModifier;
        public float ExplosionUpwardForceModifier => explosionUpwardForceModifier;
        [SerializeField]
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

            submergedDimensionDepth = floatingCollider.bounds.size.y;
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
            float surfaceHeight = Fluid.SampleSurfaceHeight(Rigidbody.position) ?? 0;
            // TODO: issue: surface height keeps increasing - due to honey material config, adjust this
            //Debug.Log($"Surface height is {surfaceHeight}");
            float worldSpaceY = Rigidbody.position.y - offsetY;
            //Debug.Log($"World space Y is {worldSpaceY}");
            float submergedDepth = surfaceHeight - worldSpaceY;
            //Debug.Log($"Submerged depth is {submergedDepth}");
            float unclampedSubmergedRatio = submergedDepth / submergedDimensionDepth;
            //Debug.Log($"Unclamped submerged ratio is {unclampedSubmergedRatio}");
            float clampedSubmergedRatio = Mathf.Clamp01(unclampedSubmergedRatio);
            //Debug.Log($"Clamped submerged ratio is {clampedSubmergedRatio}");
            float roughSubmergedVolume = clampedSubmergedRatio * Volume;
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
