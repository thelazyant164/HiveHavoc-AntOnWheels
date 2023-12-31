using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Player;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Timescale;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Gameplay;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels
{
    public enum GameState
    {
        InProgress,
        Pause,
        Win,
        Lose
    }

    public sealed class GameManager : Singleton<GameManager>, IServiceProvider<VehicleMovement>
    {
        internal VehicleMovement Vehicle { get; private set; }

        public VehicleMovement Service => Vehicle;

        public event EventHandler<GameState> OnGameStateChange;
        private CheckpointManager checkpointManager;
        private TimescaleManager timescaleManager;
        private PauseManager pauseManager;

        internal event EventHandler OnRespawn;
        public event EventHandler<VehicleMovement> OnAvailable;

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

        private void Start()
        {
            checkpointManager = CheckpointManager.Instance;
            timescaleManager = TimescaleManager.Instance;
            pauseManager = PauseManager.Instance;

            pauseManager.OnTogglePause += (object sender, bool pause) =>
                OnGameStateChange?.Invoke(sender, pause ? GameState.Pause : GameState.InProgress);
        }

        public void Register(VehicleMovement service)
        {
            Vehicle = service;
            OnAvailable?.Invoke(this, Vehicle);
        }

        internal void RegisterGate(GateTimer gate) =>
            gate.OnClose += (object sender, EventArgs e) =>
                OnGameStateChange?.Invoke(sender, GameState.Lose);

        internal void RegisterTerminalTrigger(TerminalStateTrigger trigger) =>
            trigger.OnTerminate += (object sender, TerminalState state) =>
            {
                if (state == TerminalState.Lose)
                {
                    Vehicle.Respawn(checkpointManager.LatestCheckpoint);
                    OnRespawn?.Invoke(this, EventArgs.Empty);
                    return;
                }
                OnGameStateChange?.Invoke(sender, GameState.Win);
            };

        private void HandleGameStateChange(object sender, GameState state)
        {
            switch (state)
            {
                case GameState.InProgress:
                    timescaleManager.RestoreTimescale();
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                case GameState.Pause:
                    timescaleManager.Pause();
                    Cursor.lockState = CursorLockMode.Confined;
                    break;
                case GameState.Win:
                    timescaleManager.Pause();
                    Cursor.lockState = CursorLockMode.Confined;
                    PlayerControllerManager.Instance.Reset();
                    OnGameStateChange -= HandleGameStateChange;
                    break;
                case GameState.Lose:
                    timescaleManager.Pause();
                    Cursor.lockState = CursorLockMode.Confined;
                    PlayerControllerManager.Instance.Reset();
                    OnGameStateChange -= HandleGameStateChange;
                    break;
                default:
                    break;
            }
        }
    }
}
