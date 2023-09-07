using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid;
using System.Linq;
using System;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public sealed class BuoyantExplosive : Explosive, IFloatableBody
    {
        public Rigidbody Rigidbody { get; private set; }
        public int FloatPoints { get; private set; }

        public float Volume { get; private set; }

        [Header("Buoyancy config")]
        [SerializeField]
        private float density;
        public float Density => density;

        [SerializeField]
        private float ratioToBoundVolume;
        public float RatioToBoundVolume => ratioToBoundVolume;

        protected override void Awake()
        {
            base.Awake();
            ResetMass();
            RegisterFloatPoints();
        }

        public void ResetMass()
        {
            Rigidbody = GetComponent<Rigidbody>();

            Collider floatingCollider = GetComponent<Collider>();
            Volume = floatingCollider.GetBoundVolume() * ratioToBoundVolume;
            Rigidbody.mass = density * Volume;
        }

        public void RegisterFloatPoints() =>
            FloatPoints = GetComponentsInChildren<BuoyantPoint>().Length;
    }
}
