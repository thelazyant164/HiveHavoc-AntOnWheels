using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    [RequireComponent(typeof(Canvas)), RequireComponent(typeof(CanvasScaler))]
    public sealed class UIManager : Singleton<UIManager>
    {
        private GameManager gameManager;

        [SerializeField]
        private GameObject winScreen,
            loseScreen;
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
                    break;
                case GameState.Win:
                    gameManager.OnGameStateChange -= ShowGameStateUI;
                    EndGame(state);
                    break;
                case GameState.Lose:
                    break;
                default:
                    break;
            }
        }

        private void EndGame(GameState state)
        {
            HealthBar.gameObject.SetActive(false);
            if (state == GameState.Win)
            {
                winScreen.SetActive(true);
            }
            else
            {
                loseScreen.SetActive(true);
            }
        }
    }
}
