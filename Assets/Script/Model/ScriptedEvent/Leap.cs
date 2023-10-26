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
        private float overrideThrusterForce;

        [SerializeField]
        private float overrideMotorForce;

        protected override void TriggerCallback()
        {
            GameManager.Instance.Vehicle.ThrusterForce = overrideThrusterForce;
            GameManager.Instance.Vehicle.MotorForce = overrideMotorForce;
            scriptedEventManager.AdjustScreenSplit(Camera.SplitConfiguration.VerticalDriverOnly);
            cameraManager[Role.Driver].SwitchCamera(leapCamera);
        }
    }
}
