using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    [RequireComponent(typeof(Collider))]
    public sealed class TrapTrigger : MonoBehaviour, ITrigger<TrapTrigger>
    {
        [SerializeField]
        private LayerMask receptible;
        public LayerMask Receptible => receptible;

        public event EventHandler OnTrigger;
        public event EventHandler OnTerminate;

        private bool triggered = false;

        [SerializeField]
        private bool playOnceOnly = false;

        private void Awake()
        {
            OnTrigger += (object sender, EventArgs e) => triggered = true;
        }

        private void Start()
        {
            if (!playOnceOnly)
            {
                GameManager.Instance.OnRespawn += (object sender, EventArgs e) => triggered = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!triggered && other.gameObject.InLayerMask(receptible))
            {
                // Debug.LogWarning($"Trigger activated on contact with {collision.gameObject}");
                OnTrigger?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void InvokeOnTerminate()
        {
            // Debug.LogWarning("Torpedo destroyed");
            OnTerminate?.Invoke(this, EventArgs.Empty);
        }
    }
}
