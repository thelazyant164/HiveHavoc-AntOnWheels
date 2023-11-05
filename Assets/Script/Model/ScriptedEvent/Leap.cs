using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class Leap : ScriptedEvent<Checkpoint>
    {
        [SerializeField]
        private CinemachineVirtualCamera leapCamera;

        [Space]
        [Header("Value override")]
        [SerializeField]
        private float overrideThrusterModifier;

        [SerializeField]
        private float overrideSpeedModifier;

        [SerializeField]
        private float overrideSteerModifier;

        [SerializeField]
        private float overrideAirLinearDragModifier;

        [Space]
        [Header("Screen split")]
        [SerializeField]
        private SplitPreset emphasis = SplitPreset.VerticalDriverOnly;

        protected override void TriggerCallback()
        {
            VehicleMovement vehicle = GameManager.Instance.Vehicle;

            vehicle.ThrusterModifier = overrideThrusterModifier;
            vehicle.SpeedModifier = overrideSpeedModifier;
            vehicle.SteerModifier = overrideSteerModifier;
            vehicle.AirLinearDragModifier = overrideAirLinearDragModifier;
            AdjustScreenSplit(emphasis);
            cameraManager[Role.Driver].SwitchCamera(leapCamera);
        }
    }
}
