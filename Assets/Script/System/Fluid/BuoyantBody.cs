using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public sealed class BuoyantBody : MonoBehaviour, IFloatableBody, IMovable
    {
        public Rigidbody Rigidbody { get; private set; }
        public int FloatPoints { get; private set; }
        public int SubmersedFloatPoint { get; private set; } = 0;
        public float Volume { get; private set; }

        [SerializeField]
        private float density;
        public float Density => density;

        [SerializeField]
        private float ratioToBoundVolume;
        public float RatioToBoundVolume => ratioToBoundVolume;

        [SerializeField]
        private float explosionUpwardForceModifier;
        public float ExplosionUpwardForceModifier => explosionUpwardForceModifier;

        private void Awake()
        {
            ResetMass();
            RegisterFloatPoints();
        }

        public void ReactTo<T>(Explosion<T> explosion) =>
            Rigidbody.AddExplosionForce(
                explosion.force,
                explosion.epicenter,
                explosion.radius,
                explosionUpwardForceModifier,
                ForceMode.Force
            );

        public void RegisterFloatPoints() =>
            FloatPoints = GetComponentsInChildren<BuoyantPoint>().Length;

        public void ResetMass()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Collider floatingCollider = GetComponent<Collider>();
            Volume = floatingCollider.GetBoundVolume() * ratioToBoundVolume;
            Rigidbody.mass = density * Volume;
        }
    }
}
