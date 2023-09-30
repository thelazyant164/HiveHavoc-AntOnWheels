using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Timescale;
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
        protected TimescaleManager timescaleManager;
        protected CameraManager cameraManager;

        private ITrigger<T> trigger;

        protected virtual void Awake()
        {
            trigger = GetComponent<ITrigger<T>>();
            trigger.OnTrigger += (object sender, EventArgs e) => TriggerCallback();
            trigger.OnTerminate += (object sender, EventArgs e) => TerminateCallback();
        }

        private void Start()
        {
            cameraManager = CameraManager.Instance;
            timescaleManager = TimescaleManager.Instance;
            scriptedEventManager = ScriptedEventManager.Instance;
        }

        protected virtual void TriggerCallback() { }

        protected virtual void TerminateCallback() { }
    }
}
