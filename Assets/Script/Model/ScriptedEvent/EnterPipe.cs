using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class EnterPipe : ScriptedEvent<Checkpoint>
    {
        [SerializeField]
        private CinemachineVirtualCamera pipeSequenceDriverCamera;

        [SerializeField]
        private CinemachineVirtualCamera pipeSequenceShooterCamera;

        protected override void TriggerCallback()
        {
            Debug.LogWarning("Enter pipe");
            scriptedEventManager.AdjustScreenSplit(Camera.SplitConfiguration.HorizontalEven);
            cameraManager[Role.Driver].SwitchCamera(pipeSequenceDriverCamera);
            cameraManager[Role.Shooter].SwitchCamera(pipeSequenceShooterCamera);
        }
    }
}
