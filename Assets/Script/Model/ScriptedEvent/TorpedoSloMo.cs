using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class TorpedoSloMo : ScriptedEvent<TorpedoTrigger>
    {
        [SerializeField]
        private float sloMoTimeScale = .5f;

        protected override void TriggerCallback()
        {
            scriptedEventManager.AdjustScreenSplit(
                Camera.SplitConfiguration.VerticalShooterEmphasis
            );
            Time.timeScale = sloMoTimeScale;
        }

        protected override void TerminateCallback()
        {
            scriptedEventManager.AdjustScreenSplit(Camera.SplitConfiguration.VerticalEven);
            Time.timeScale = 1;
        }
    }
}
