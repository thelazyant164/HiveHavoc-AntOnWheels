using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Unnamed.RacingGame
{
    public sealed class PlayerManager : Singleton<PlayerManager>
    {
        public Shooter.Shooter Shooter { get; private set; }

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

            Shooter = GetComponentInChildren<Shooter.Shooter>();
        }
    }

}
