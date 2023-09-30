using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Timescale
{
    public struct TimescaleModifier
    {
        public float newTimescale;
        public float oldTimescale;

        public TimescaleModifier(float oldTimescale, float newTimescale)
        {
            this.oldTimescale = oldTimescale;
            this.newTimescale = newTimescale;
        }

        public void Apply()
        {
            Time.timeScale = newTimescale;
        }

        public void Revert()
        {
            Time.timeScale = oldTimescale;
        }
    }

    public sealed class TimescaleManager : Singleton<TimescaleManager>
    {
        private Stack<TimescaleModifier> modifiers = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError(
                    "There's more than one InputManager! " + transform + " - " + Instance
                );
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            while (modifiers.Count > 0)
            {
                RestoreTimescale();
            }
        }

        internal void AdjustTimescale(float newTimescale)
        {
            TimescaleModifier modifier = new TimescaleModifier(Time.timeScale, newTimescale);
            modifier.Apply();
            modifiers.Push(modifier);
        }

        internal void RestoreTimescale()
        {
            if (modifiers.TryPop(out TimescaleModifier modifier))
            {
                modifier.Revert();
            }
        }
    }
}
