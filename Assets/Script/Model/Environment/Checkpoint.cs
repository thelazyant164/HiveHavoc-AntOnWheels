using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn
{
    [RequireComponent(typeof(Collider))]
    public sealed class Checkpoint
        : MonoBehaviour,
            ITrigger<Checkpoint>,
            IServiceProvider<Checkpoint>,
            IService<Checkpoint>
    {
        [SerializeField]
        private LayerMask receptible;
        public LayerMask Receptible => receptible;

        public event EventHandler OnTrigger;
        public event EventHandler OnTerminate;
        public event EventHandler<Checkpoint> OnAvailable;

        private void Awake()
        {
            OnAvailable?.Invoke(this, this); // silence warning
        }

        private void Start()
        {
            Register(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.InLayerMask(receptible))
            {
                OnTrigger?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnDestroy()
        {
            OnTerminate?.Invoke(this, EventArgs.Empty);
        }

        public void Register(Checkpoint service)
        {
            CheckpointManager.Instance.Register(this);
        }

        public void Register(IServiceProvider<Checkpoint> provider)
        {
            CheckpointManager.Instance.Register(this);
        }
    }
}
