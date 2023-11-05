using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
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

        [SerializeField]
        private SplitPreset emphasis = SplitPreset.VerticalShooterEmphasis;

        [SerializeField]
        private SplitPreset normal = SplitPreset.VerticalEven;

        protected override void TriggerCallback()
        {
            AdjustScreenSplit(emphasis);
            timescaleManager.AdjustTimescale(sloMoTimeScale);

            if (dangerAlert != null)
                UIManager.Instance.VocalAudio.PlayOneShot(dangerAlert);
        }

        protected override void TerminateCallback()
        {
            AdjustScreenSplit(normal);
            timescaleManager.RestoreTimescale();
        }
    }
}
