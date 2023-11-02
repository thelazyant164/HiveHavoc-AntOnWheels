using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class Leap : ScriptedEvent<Checkpoint>
    {
        [SerializeField]
        private CinemachineVirtualCamera leapCamera;

        [SerializeField]
        private float overrideThrusterModifier;

        [SerializeField]
        private float overrideSpeedModifier;

        [SerializeField]
        private float overrideAirLinearDragModifier;

        protected override void TriggerCallback()
        {
            GameManager.Instance.Vehicle.ThrusterModifier = overrideThrusterModifier;
            GameManager.Instance.Vehicle.SpeedModifier = overrideSpeedModifier;
            GameManager.Instance.Vehicle.AirLinearDragModifier = overrideAirLinearDragModifier;
            scriptedEventManager.AdjustScreenSplit(Camera.SplitConfiguration.VerticalDriverOnly);
            cameraManager[Role.Driver].SwitchCamera(leapCamera);
        }
    }
}
