using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
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

        [SerializeField]
        private SplitPreset pipeSplit = SplitPreset.HorizontalEven;

        protected override void TriggerCallback()
        {
            Debug.LogWarning("Enter pipe");
            AdjustScreenSplit(pipeSplit);
            cameraManager[Role.Driver].SwitchCamera(pipeSequenceDriverCamera);
            cameraManager[Role.Shooter].SwitchCamera(pipeSequenceShooterCamera);
        }
    }
}
