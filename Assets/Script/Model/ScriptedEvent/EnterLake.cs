using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
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
