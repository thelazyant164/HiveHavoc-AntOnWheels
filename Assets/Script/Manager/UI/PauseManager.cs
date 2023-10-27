using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public sealed class PauseManager : Singleton<PauseManager>
    {
        private bool pause = false;
        internal event EventHandler<bool> OnTogglePause;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError(
                    "There's more than one PauseManager! " + transform + " - " + Instance
                );
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            Shooter.Shooter shooter = PlayerManager.Instance.Shooter;
            Driver.Driver driver = PlayerManager.Instance.Driver;

            shooter.OnPause += TogglePause;
            driver.OnPause += TogglePause;
        }

        private void TogglePause(object sender, EventArgs e)
        {
            pause = !pause;
            AudioListener.pause = pause;
            OnTogglePause?.Invoke(sender, pause);
        }

        internal void Unpause()
        {
            pause = false;
            AudioListener.pause = pause;
            OnTogglePause?.Invoke(this, pause);
        }
    }
}
