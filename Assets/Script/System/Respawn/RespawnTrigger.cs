using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn
{
    [RequireComponent(typeof(Collider))]
    public sealed class RespawnTrigger
        : MonoBehaviour,
            ITrigger<RespawnTrigger>,
            IService<RespawnTrigger>
    {
        [SerializeField]
        private LayerMask receptible;
        public LayerMask Receptible => receptible;

        [SerializeReference]
        private List<GameObject> respawnables;
        internal List<GameObject> Respawnables => respawnables;

        public event EventHandler OnTrigger;
        public event EventHandler OnTerminate;

        private void Awake()
        {
            OnTerminate?.Invoke(this, EventArgs.Empty); // silence warning
        }

        private void Start()
        {
            Register(RespawnManager.Instance);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.InLayerMask(receptible))
            {
                OnTrigger?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Register(IServiceProvider<RespawnTrigger> provider) => provider.Register(this);
    }
}
