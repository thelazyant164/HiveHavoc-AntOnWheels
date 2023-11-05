using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class DestructibleBlockade
        : MonoBehaviour,
            IDestructible<DestructibleBlockade>,
            IRespawnable
    {
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        [SerializeField]
        private LayerMask destroyedBy;
        public LayerMask DestroyedBy => destroyedBy;

        public event EventHandler<DestructibleBlockade> OnDestroy;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.InLayerMask(destroyedBy))
            {
                Destroy();
            }
        }

        public void Destroy()
        {
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }
    }
}
