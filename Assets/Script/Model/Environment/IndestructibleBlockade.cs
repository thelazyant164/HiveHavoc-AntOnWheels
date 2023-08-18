using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class IndestructibleBlockade : MonoBehaviour, IMovable
    {
        private Rigidbody rb;

        private void Awake() => rb = GetComponent<Rigidbody>();

        public void ReactTo(Explosion explosion) => rb.AddExplosionForce(explosion.force, explosion.epicenter, explosion.radius, 0f, ForceMode.Force);
    }
}
