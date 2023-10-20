using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class EnterLake : ScriptedEvent<Checkpoint>
    {
        [SerializeField]
        private CinemachineVirtualCamera lakeSequenceDriverCamera;

        protected override void TriggerCallback()
        {
            scriptedEventManager.AdjustScreenSplit(Camera.SplitConfiguration.VerticalEven);
            cameraManager[Role.Driver].SwitchCamera(lakeSequenceDriverCamera);
        }
    }
}
