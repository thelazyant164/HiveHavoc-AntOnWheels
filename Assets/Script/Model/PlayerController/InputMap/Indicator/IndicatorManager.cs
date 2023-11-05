using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    [Serializable]
    public sealed class RoleDictionary : SerializableDictionary<Role, RectTransform> { }

    public sealed class IndicatorManager : Singleton<IndicatorManager>
    {
        [SerializeField]
        private TextMeshProUGUI timer;

        [SerializeField]
        private float indicatorSwitchTime = .5f;

        [SerializeField]
        private float countdown = 3f;

        [SerializeField]
        private RoleDictionary roles;

        [SerializeField]
        private string gameScene;

        private IEnumerable<ControllerIndicator> controllers;
        private Dictionary<ControllerIndicator, ControllerMap> ready = new();
        private Coroutine countdownToGame;

        public event EventHandler<InputMap> OnReady;

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

            timer.gameObject.SetActive(false);

            controllers = GetComponentsInChildren<ControllerIndicator>();
            foreach (ControllerIndicator controller in controllers)
            {
                controller.OnSwitch += Switch;
                controller.OnReady += Ready;
            }
        }

        private void Start()
        {
            PlayerControllerManager.Instance.Register(this);
        }

        private void OnDestroy()
        {
            PlayerControllerManager.Instance.Unregister(this);
        }

        private void Switch(object sender, Direction direction)
        {
            ControllerIndicator indicator = sender as ControllerIndicator;
            int newRole = (int)indicator.Role + (int)direction;
            if (Enum.IsDefined(typeof(Role), newRole))
            {
                indicator.MoveTo((Role)newRole, roles[(Role)newRole], indicatorSwitchTime);
            }
        }

        private void Ready(object sender, ControllerMap mapping)
        {
            ControllerIndicator indicator = sender as ControllerIndicator;
            if (mapping.ready)
            {
                if (ready.TryAdd(indicator, mapping) && IsValidRoster() && countdownToGame == null)
                {
                    // issue #2: as each role starting role defaults to driver, 1 player moves to other side -> immediately valid roster (eventho visually, 1 player still undecided)
                    countdownToGame = StartCoroutine(StartGameCountdown(countdown));
                }
            }
            else
            {
                ready.Remove(indicator);
                if (countdownToGame != null)
                {
                    timer.gameObject.SetActive(false);
                    StopCoroutine(countdownToGame);
                    countdownToGame = null;
                }
            }
        }

        private bool IsValidRoster() =>
            ready.Values.GroupBy(controllerMap => controllerMap.role).Count()
            == controllers.Count();

        private IEnumerator StartGameCountdown(float duration)
        {
            OnReady?.Invoke(this, new InputMap(ready.Values));
            timer.gameObject.SetActive(true);
            while (duration > 0)
            {
                duration -= Time.deltaTime;
                timer.SetText(Math.Ceiling(duration).ToString());
                yield return null;
            }
            SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Single);
        }
    }
}
