using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class IndestructibleBlockade : MonoBehaviour, IMovable
    {
        private Rigidbody rb;

        [SerializeField]
        private float explosionUpwardForceModifier;
        public float ExplosionUpwardForceModifier => explosionUpwardForceModifier;

        private void Awake() => rb = GetComponent<Rigidbody>();

        public void ReactTo<T>(Explosion<T> explosion) =>
            rb.AddExplosionForce(
                explosion.force,
                explosion.epicenter,
                explosion.radius,
                explosionUpwardForceModifier,
                ForceMode.Force
            );
    }
}
