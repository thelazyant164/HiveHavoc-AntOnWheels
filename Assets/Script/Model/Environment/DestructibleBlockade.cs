using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    [RequireComponent(typeof (Rigidbody))]
    public sealed class DestructibleBlockade : MonoBehaviour, IDestructible
    {
        public event EventHandler<IDestructible> OnDestroy;

        public void Destroy()
        {
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }
    }
}
