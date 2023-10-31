using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public sealed class UIManager : Singleton<UIManager>
    {
        private GameManager gameManager;
        internal Crosshair Crosshair { get; private set; }
        internal VehicleHealthBar HealthBar { get; private set; }

        [SerializeField]
        private AudioSource uiAudio;
        internal AudioSource UIAudio => uiAudio;

        [SerializeField]
        private AudioSource vocalAudio;
        internal AudioSource VocalAudio => vocalAudio;
        private List<Scene> openedPauseScenesBuffer = new();

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

            Crosshair = GetComponentInChildren<Crosshair>();
            HealthBar = GetComponentInChildren<VehicleHealthBar>();

            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
            {
                switch (scene.name)
                {
                    case "ConfirmQuitMenu"
                    or "MappingMenu"
                    or "OptionsMenu":
                        openedPauseScenesBuffer.Add(scene);
                        break;
                    default:
                        break;
                }
            };

            SceneManager.sceneUnloaded += (Scene scene) =>
            {
                switch (scene.name)
                {
                    case "ConfirmQuitMenu"
                    or "MappingMenu"
                    or "OptionsMenu":
                        openedPauseScenesBuffer.Remove(scene);
                        break;
                    default:
                        break;
                }
            };
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.OnGameStateChange += ShowGameStateUI;
        }

        private void CloseAllPausedScenes()
        {
            foreach (Scene scene in openedPauseScenesBuffer)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
            openedPauseScenesBuffer.Clear();
        }

        private void ShowGameStateUI(object sender, GameState state)
        {
            switch (state)
            {
                case GameState.InProgress:
                    SceneManager.UnloadSceneAsync("PauseMenu");
                    CloseAllPausedScenes();
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
