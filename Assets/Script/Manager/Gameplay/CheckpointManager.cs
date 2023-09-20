using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Gameplay
{
    public sealed class CheckpointManager : Singleton<CheckpointManager>
    {
        [SerializeField]
        private Checkpoint latestCheckpoint;
        internal Transform LatestCheckpoint => latestCheckpoint.transform;

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

        internal void RegisterCheckpoint(Checkpoint checkpoint) =>
            checkpoint.OnTrigger += (object sender, EventArgs e) => latestCheckpoint = checkpoint;
    }
}
