using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class DestructibleBlockade : MonoBehaviour, IDestructible<DestructibleBlockade>
    {
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        public event EventHandler<DestructibleBlockade> OnDestroy;

        public void Destroy()
        {
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }
    }
}
