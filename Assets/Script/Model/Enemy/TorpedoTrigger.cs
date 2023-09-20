using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    [RequireComponent(typeof(Collider))]
    public sealed class TorpedoTrigger : MonoBehaviour, ITrigger<TorpedoTrigger>
    {
        [SerializeField]
        private LayerMask receptible;
        public LayerMask Receptible => receptible;

        public event EventHandler OnTrigger;
        public event EventHandler OnTerminate;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.InLayerMask(receptible))
            {
                // Debug.LogWarning($"Trigger activated on contact with {collision.gameObject}");
                OnTrigger?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void InvokeOnTerminate()
        {
            Debug.LogWarning("End slomo");
            OnTerminate?.Invoke(this, EventArgs.Empty);
        }
    }
}
