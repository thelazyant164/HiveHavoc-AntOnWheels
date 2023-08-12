using Com.Unnamed.RacingGame.Driver;
using Com.Unnamed.RacingGame.Shooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Unnamed.RacingGame
{
    public sealed class GameManager : Singleton<GameManager>
    {
        internal VehicleMovement Vehicle { get; private set; }

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

        internal void RegisterVehicle(VehicleMovement vehicle) => Vehicle = vehicle;
    }
}
