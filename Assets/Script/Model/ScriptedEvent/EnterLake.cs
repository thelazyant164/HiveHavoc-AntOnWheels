using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class EnterLake : ScriptedEvent<Checkpoint>
    {
        [SerializeField]
        private CinemachineVirtualCamera lakeSequenceDriverCamera;

        [SerializeField]
        private CinemachineVirtualCamera lakeSequenceShooterCamera;

        protected override void TriggerCallback()
        {
            cameraManager[Role.Driver].SwitchCamera(lakeSequenceDriverCamera);
            cameraManager[Role.Shooter].SwitchCamera(lakeSequenceShooterCamera);
        }
    }
}
