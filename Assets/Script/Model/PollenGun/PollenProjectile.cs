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
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        private Rigidbody rb;

        [SerializeField]
        private float damage;
        public float Damage => damage;

        [SerializeField]
        private LayerMask interceptedBy;
        public LayerMask InterceptedBy => interceptedBy;
        public LayerMask DestroyedBy => interceptedBy;

        public IDamaging.Target TargetType => IDamaging.Target.Enemy;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Launch(Vector3 spatialImpulse) =>
            rb.AddForce(spatialImpulse, ForceMode.Impulse);

        public abstract void Destroy();
    }
}
