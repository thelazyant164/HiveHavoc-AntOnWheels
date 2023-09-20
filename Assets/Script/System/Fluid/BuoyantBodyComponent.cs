using System.Collections;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid
{
    [RequireComponent(typeof(Collider))]
    public sealed class BuoyantBodyComponent : MonoBehaviour, IFloatableBody, IMovable
    {
        private IMovable movableParent;

        public Rigidbody Rigidbody { get; private set; }

        public float SubmergedDimensionDepth { get; private set; }
        public int FloatPoints { get; private set; }
        public float Volume { get; private set; }

        [SerializeField]
        private float density;
        public float Density => density;

        [SerializeField]
        private float ratioToBoundVolume;
        public float RatioToBoundVolume => ratioToBoundVolume;

        public float ExplosionUpwardForceModifier => 0;

        private void Awake()
        {
            ResetMass();
            RegisterFloatPoints();
        }

        private void Start()
        {
            IFloatableBody body = transform.parent.GetComponent<IFloatableBody>();
            if (body is IMovable movable)
            {
                movableParent = movable;
            }
            else
            {
                movableParent = transform.parent.GetComponentInParent<IMovable>();
            }
            Rigidbody = body.Rigidbody;
            if (Rigidbody == null)
            {
                Debug.LogError($"{this} does not have a parent BuoyantBody with a Rigidbody!");
            }
        }

        public void RegisterFloatPoints() =>
            FloatPoints = GetComponentsInChildren<BuoyantPoint>().Length;

        public void ResetMass()
        {
            Collider floatingCollider = GetComponent<Collider>();
            Volume = floatingCollider.GetBoundVolume() * ratioToBoundVolume;
            SubmergedDimensionDepth = floatingCollider.bounds.size.y;
        }

        public void ReactTo<T>(Explosion<T> explosion) => movableParent.ReactTo(explosion);
    }
}
