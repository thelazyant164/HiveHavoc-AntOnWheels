using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    [RequireComponent(typeof(Canvas)), RequireComponent(typeof(CanvasScaler))]
    public sealed class UIManager : Singleton<UIManager>
    {
        private GameManager gameManager;
        internal VehicleHealthBar HealthBar { get; private set; }

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

            HealthBar = GetComponentInChildren<VehicleHealthBar>();
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.OnGameStateChange += ShowGameStateUI;
        }

        private void ShowGameStateUI(object sender, GameState state)
        {
            switch (state)
            {
                case GameState.InProgress:
                    SceneManager.UnloadSceneAsync("PauseMenu");
                    break;
                case GameState.Pause:
                    SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
                    break;
                case GameState.Win:
                    gameManager.OnGameStateChange -= ShowGameStateUI;
                    SceneManager.LoadSceneAsync("WinMenu", LoadSceneMode.Additive);
                    break;
                case GameState.Lose:
                    gameManager.OnGameStateChange -= ShowGameStateUI;
                    SceneManager.LoadSceneAsync("LoseMenu", LoadSceneMode.Additive);
                    break;
                default:
                    break;
            }
        }
    }
}
