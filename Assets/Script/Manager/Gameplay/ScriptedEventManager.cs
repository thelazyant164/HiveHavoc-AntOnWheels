using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class ScriptedEventManager : Singleton<ScriptedEventManager>
    {
        private SplitManager screenSplit;

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

        private void Start()
        {
            screenSplit = SplitManager.Instance;
            screenSplit.Apply(SplitConfiguration.VerticalEven);
        }

        internal void AdjustScreenSplit(SplitConfiguration config) => screenSplit.Apply(config);
    }
}
