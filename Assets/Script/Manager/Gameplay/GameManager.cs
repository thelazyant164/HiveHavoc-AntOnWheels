using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Player;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels
{
    public enum GameState
    {
        InProgress,
        Win,
        Lose
    }

    public sealed class GameManager : Singleton<GameManager>
    {
        internal VehicleMovement Vehicle { get; private set; }
        public event EventHandler<GameState> OnGameStateChange;

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

            OnGameStateChange += HandleGameStateChange;
        }

        internal void RegisterVehicle(VehicleMovement vehicle) => Vehicle = vehicle;

        internal void RegisterVehicle(VehicleHealth vehicle) =>
            vehicle.OnDeath += (object sender, EventArgs e) =>
                OnGameStateChange?.Invoke(sender, GameState.Lose);

        private void HandleGameStateChange(object sender, GameState state)
        {
            switch (state)
            {
                case GameState.InProgress:
                    break;
                case GameState.Win:
                    Time.timeScale = 0;
                    OnGameStateChange -= HandleGameStateChange;
                    break;
                case GameState.Lose:
                    OnGameStateChange -= HandleGameStateChange;
                    break;
                default:
                    break;
            }
        }
    }
}
