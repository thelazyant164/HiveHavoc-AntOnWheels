using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
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

        [SerializeField]
        private Transform target;
        private GameObject cachedTarget;

        private void Awake()
        {
            OnTrigger += (object sender, EventArgs e) => triggered = true;
            cachedTarget = target.gameObject;
        }

        private void Start()
        {
            if (!playOnceOnly)
            {
                GameManager.Instance.OnRespawn += (object sender, EventArgs e) => triggered = false;
            }
            if (target.TryGetComponent(out IRespawnable respawnable))
            {
                RespawnManager.Instance.OnRespawn += (object sender, GameObject respawned) =>
                {
                    if (sender == (object)cachedTarget)
                    {
                        target = respawned.transform;
                    }
                };
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!triggered && other.gameObject.InLayerMask(receptible))
            {
                // Debug.LogWarning($"Trigger activated on contact with {collision.gameObject}");
                if (target != null)
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
