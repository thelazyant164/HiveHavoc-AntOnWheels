using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class TorpedoSloMo : ScriptedEvent<TrapTrigger>
    {
        [SerializeField]
        private float sloMoTimeScale = .5f;

        [SerializeField]
        private AudioClip dangerAlert;

        protected override void TriggerCallback()
        {
            scriptedEventManager.AdjustScreenSplit(
                Camera.SplitConfiguration.VerticalShooterEmphasis
            );
            timescaleManager.AdjustTimescale(sloMoTimeScale);

            if (dangerAlert != null)
                UIManager.Instance.VocalAudio.PlayOneShot(dangerAlert);
        }

        protected override void TerminateCallback()
        {
            scriptedEventManager.AdjustScreenSplit(Camera.SplitConfiguration.VerticalEven);
            timescaleManager.RestoreTimescale();
        }
    }
}
