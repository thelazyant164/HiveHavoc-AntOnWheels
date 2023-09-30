using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
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
            timescaleManager.AdjustTimescale(sloMoTimeScale);
        }

        protected override void TerminateCallback()
        {
            scriptedEventManager.AdjustScreenSplit(Camera.SplitConfiguration.VerticalEven);
            timescaleManager.RestoreTimescale();
        }
    }
}
