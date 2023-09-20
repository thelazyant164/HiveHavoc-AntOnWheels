using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    [RequireComponent(typeof(ITrigger))]
    public abstract class ScriptedEvent<T> : MonoBehaviour where T : ITrigger<T>
    {
        [SerializeField]
        private LayerMask receptible;
        public LayerMask Receptible => receptible;
        protected ScriptedEventManager scriptedEventManager;

        private ITrigger<T> trigger;

        private void Awake()
        {
            trigger = GetComponent<ITrigger<T>>();
            trigger.OnTrigger += (object sender, EventArgs e) => TriggerCallback();
            trigger.OnTerminate += (object sender, EventArgs e) => TerminateCallback();
        }

        private void Start()
        {
            scriptedEventManager = ScriptedEventManager.Instance;
        }

        protected abstract void TriggerCallback();
        protected abstract void TerminateCallback();
    }
}
