using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public enum Direction
    {
        Left = -1,
        Right = 1
    }

    [Serializable]
    public struct ControllerMap
    {
        public List<InputDevice> pairedDevices;
        public Controller controller;
        public Role role;
        public bool ready;

        public static ControllerMap Default => new ControllerMap();

        public bool Empty => pairedDevices == null || pairedDevices.Count == 0;
    }

    public sealed class ControllerIndicator : PlayerController
    {
        [SerializeField]
        private GameObject leftPrompt,
            rightPrompt,
            ready,
            missing;

        private RectTransform rect;
        private InputAction switchAction;
        private InputAction readyAction;
        private InputAction unreadyAction;
        internal Role Role { get; private set; }

        internal event EventHandler<Direction> OnSwitch;
        internal event EventHandler<ControllerMap> OnReady;

        protected override void Awake()
        {
            base.Awake();
            rect = GetComponent<RectTransform>();
            ready.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            RequestPairing();
        }

        protected override void BindCallbackToAction()
        {
            if (switchAction != null)
                switchAction.started += Switch;
            if (readyAction != null)
                readyAction.started += Ready;
            if (unreadyAction != null)
                unreadyAction.started += Unready;
        }

        protected override void UnbindCallbackFromAction()
        {
            if (switchAction != null)
                switchAction.started -= Switch;
            if (readyAction != null)
                readyAction.started -= Ready;
            if (unreadyAction != null)
                unreadyAction.started -= Unready;
        }

        protected override void MapSchemeTo(InputActionAsset action)
        {
            InputActionMap pick = action.FindActionMap("Pick");
            switchAction = pick.FindAction("Switch");
            readyAction = pick.FindAction("Ready");
            unreadyAction = pick.FindAction("Unready");
        }

        protected override void OnPairingSucceed()
        {
            base.OnPairingSucceed();
            missing.SetActive(false);
        }

        protected override void OnDeviceMissing()
        {
            base.OnDeviceMissing();
            missing.SetActive(true);
            OnReady?.Invoke(this, ControllerMap.Default);
        }

        private void Switch(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            if (context.ReadValue<float>() > 0)
            {
                OnSwitch?.Invoke(this, Direction.Right);
            }
            else
            {
                OnSwitch?.Invoke(this, Direction.Left);
            }
            Unready(context);
        }

        private void Ready(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            ready.SetActive(true);
            OnReady?.Invoke(this, GetControllerMap(Role));
        }

        private void Unready(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            ready.SetActive(false);
            OnReady?.Invoke(this, ControllerMap.Default);
        }

        internal void MoveTo(Role role, RectTransform target, float time)
        {
            Role = role;
            leftPrompt.SetActive(Enum.IsDefined(typeof(Role), (int)role - 1));
            rightPrompt.SetActive(Enum.IsDefined(typeof(Role), (int)role + 1));

            StartCoroutine(
                rect.LerpTo(new Vector3(target.position.x, rect.position.y, rect.position.z), time)
            );
        }
    }
}
