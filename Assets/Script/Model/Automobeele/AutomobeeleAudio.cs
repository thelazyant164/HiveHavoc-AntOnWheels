using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Audio
{
    public sealed class AutomobeeleAudio : MonoBehaviour
    {
        private Shooter.Shooter shooter;
        private Driver.Driver driver;

        [SerializeField]
        private AudioSource wingAudio;

        [SerializeField]
        private AudioSource engineAudio;

        [SerializeField]
        private AudioSource brakeAudio;

        [SerializeField]
        private float engineSFXDuration;

        private bool hasAccelerated = false;

        private void Start()
        {
            shooter = PlayerManager.Instance.Shooter;
            shooter.OnThruster += HandleThruster;

            driver = PlayerManager.Instance.Driver;
            driver.OnAccelerate += HandleAccelerate;
            driver.OnBrake += HandleBrake;
        }

        private void OnDestroy()
        {
            shooter.OnThruster -= HandleThruster;
            driver.OnAccelerate -= HandleAccelerate;
            driver.OnBrake -= HandleBrake;
        }

        private void HandleBrake(object sender, bool isBraking)
        {
            if (isBraking && !brakeAudio.isPlaying)
            {
                brakeAudio.Play();
            }
            else if (!isBraking)
            {
                brakeAudio.Stop();
            }
        }

        private void HandleAccelerate(object sender, float value)
        {
            if (!hasAccelerated && value != 0 && !engineAudio.isPlaying)
            {
                engineAudio.Play();
                hasAccelerated = true;
            }

            if (value == 0)
            {
                engineAudio.Stop();
                hasAccelerated = false;
            }
        }

        private void HandleThruster(object sender, float thruster)
        {
            if (thruster == 0)
            {
                wingAudio.Stop();
            }
            else if (!wingAudio.isPlaying)
            {
                wingAudio.Play();
            }
            wingAudio.volume = thruster;
        }
    }
}
