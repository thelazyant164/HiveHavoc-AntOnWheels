using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Timescale;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    [RequireComponent(typeof(ITrigger))]
    public abstract class ScriptedEvent<T> : MonoBehaviour, IServiceConsumer<VehicleMovement>
        where T : ITrigger<T>
    {
        [SerializeField]
        private LayerMask receptible;
        private ScriptedEventManager scriptedEventManager;
        protected TimescaleManager timescaleManager;
        protected CameraManager cameraManager;

        private Driver.Driver driver;
        private Shooter.Shooter shooter;
        private VehicleMovement vehicle;

        private ITrigger<T> trigger;

        protected virtual void Awake()
        {
            trigger = GetComponent<ITrigger<T>>();
            trigger.OnTrigger += (object sender, EventArgs e) => TriggerCallback();
            trigger.OnTerminate += (object sender, EventArgs e) => TerminateCallback();
        }

        protected virtual void Start()
        {
            Register(GameManager.Instance);
            cameraManager = CameraManager.Instance;
            timescaleManager = TimescaleManager.Instance;
            scriptedEventManager = ScriptedEventManager.Instance;
            PlayerManager players = PlayerManager.Instance;
            driver = players.Driver;
            shooter = players.Shooter;
        }

        public void Register(IServiceProvider<VehicleMovement> provider)
        {
            if (provider.Service != null)
            {
                vehicle = provider.Service;
                return;
            }

            provider.OnAvailable += (object sender, VehicleMovement vehicle) =>
            {
                this.vehicle = vehicle;
            };
        }

        protected virtual void TriggerCallback() { }

        protected virtual void TerminateCallback() { }

        protected void EnableControls(Role role, bool active)
        {
            if (role == Role.Driver)
            {
                driver.gameObject.SetActive(active);
                if (!active)
                {
                    vehicle.Reset();
                }
                return;
            }

            shooter.gameObject.SetActive(active);
        }

        protected void AdjustScreenSplit(SplitPreset preset) =>
            scriptedEventManager.AdjustScreenSplit(SplitConfiguration.GetPreset(preset));
    }
}
