using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    /// <summary>
    /// Buoyant body. Can either include a <see cref="Collider"/> and many <see cref="BuoyantPoint"/>s, or smaller <see cref="BuoyantBodyComponent"/>s.
    /// </summary>
    /// <remarks><see cref="Density"/> and <see cref="RatioToBoundVolume"/> will be overriden by children <see cref="BuoyantBodyComponent"/>s.
    /// <see cref="Volume"/> and <see cref="Rigidbody.mass"/> will be the sum of all children, assuming no physical overlap.</remarks>
    [RequireComponent(typeof(Rigidbody))]
    public sealed class BuoyantBody : MonoBehaviour, IFloatableBody, IMovable, IRespawnable
    {
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        public Rigidbody Rigidbody { get; private set; }

        public float SubmergedDimensionDepth { get; private set; }
        public int FloatPoints { get; private set; }
        public float Volume { get; private set; }

        [SerializeField]
        private bool isComposite = false;
        private Collider mainCollider;

        [SerializeField, ShowWhen("isComposite", false)]
        private float density;
        public float Density => density;

        [SerializeField, ShowWhen("isComposite", false)]
        private float ratioToBoundVolume;
        public float RatioToBoundVolume => ratioToBoundVolume;

        [SerializeField, ShowWhen("isComposite", false)]
        private float explosionUpwardForceModifier;

        public float ExplosionUpwardForceModifier => explosionUpwardForceModifier;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            if (!isComposite)
            {
                if (!TryGetComponent(out mainCollider))
                    Debug.LogError("No collider detected on BuoyantBody marked as non-composite");
            }
        }

        private void Start()
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

        public void RegisterFloatPoints()
        {
            if (!isComposite)
            {
                FloatPoints = GetComponentsInChildren<BuoyantPoint>().Length;
                return;
            }
            FloatPoints = GetComponentsInChildren<BuoyantBodyComponent>()
                .Sum(buoyantComponent => buoyantComponent.FloatPoints);
        }

        public void ResetMass()
        {
            if (!isComposite)
            {
                Volume = mainCollider.GetBoundVolume() * ratioToBoundVolume;
                Rigidbody.mass = density * Volume;

                SubmergedDimensionDepth = mainCollider.bounds.size.y;
                return;
            }

            BuoyantBodyComponent[] buoyantComponents =
                GetComponentsInChildren<BuoyantBodyComponent>();
            Volume = buoyantComponents.Sum(buoyantComponent => buoyantComponent.Volume); // assume child buoyant components don't overlap
            Rigidbody.mass = buoyantComponents.Sum(
                buoyantComponent => buoyantComponent.Volume * buoyantComponent.Density
            );
        }

        public void ResetTo(Vector3 position, Quaternion rotation)
        {
            Rigidbody.position = position;
            Rigidbody.rotation = rotation;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
