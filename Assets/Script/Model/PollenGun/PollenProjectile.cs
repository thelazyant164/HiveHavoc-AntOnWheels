using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public abstract class PollenProjectile : MonoBehaviour, IProjectile
    {
        private Rigidbody rb;

        [SerializeField]
        private float damage;
        public float Damage => damage;

        [SerializeField]
        private LayerMask blocking;
        public LayerMask Blocking => blocking;

        public IDamaging.Target TargetType => IDamaging.Target.Enemy;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Launch(Vector3 spatialImpulse) =>
            rb.AddForce(spatialImpulse, ForceMode.Impulse);
    }
}
