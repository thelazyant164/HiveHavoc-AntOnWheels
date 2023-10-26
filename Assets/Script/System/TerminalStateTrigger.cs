using System;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public enum TerminalState
    {
        Win,
        Lose
    }

    [RequireComponent(typeof(Collider))]
    public sealed class TerminalStateTrigger : MonoBehaviour, ITrigger
    {
        [SerializeField]
        private LayerMask receptible;
        public LayerMask Receptible => receptible;

        [SerializeField]
        private TerminalState state;

        internal event EventHandler<TerminalState> OnTerminate;

        private void Awake()
        {
            Collider trigger = GetComponent<Collider>();
            if (trigger == null || !trigger.isTrigger)
                Debug.LogError($"Missing trigger collider in {this}");
        }

        private void Start()
        {
            GameManager.Instance.RegisterTerminalTrigger(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.InLayerMask(receptible))
            {
                Debug.LogWarning($"Terminal state triggered on contact with {other.gameObject}");
                OnTerminate?.Invoke(this, state);
            }
        }
    }
}
