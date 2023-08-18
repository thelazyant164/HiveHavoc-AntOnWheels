using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input
{
    public abstract class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private InputActionAsset action;
        internal InputActionAsset Action => action;

        [SerializeField]
        private Controller controller;
        internal Controller Controller => controller;
        private PlayerControllerManager inputMap;
        public static event EventHandler<PlayerController> OnRequestPairing;
        public static event EventHandler<PlayerController> OnRequestUnpair;

        protected virtual void Awake()
        {
            MapSchemeTo(action);
        }

        protected virtual void Start()
        {
            inputMap = PlayerControllerManager.Instance;
            inputMap.OnDeviceAdd += AddDevice;
            inputMap.OnDeviceRemove += RemoveDevice;
        }

        private void AddDevice(object sender, InputDevice device)
        {
            if (!IsOpenForPairingWith(device))
                return;
            RequestPairing();
        }

        private void RemoveDevice(object sender, InputDevice device)
        {
            if (!IsMappedToSelf(device))
                return;
            UnpairFrom(device);
        }

        protected virtual void OnEnable() => BindCallbackToAction();

        protected virtual void OnDisable() => UnbindCallbackFromAction();

        protected virtual void OnDestroy()
        {
            inputMap.OnDeviceAdd -= AddDevice;
            inputMap.OnDeviceRemove -= RemoveDevice;
            RequestUnpair();
        }

        protected ControllerMap GetControllerMap(Role role)
        {
            ControllerMap newMapping = ControllerMap.Default;
            newMapping.pairedDevices = inputMap.GetPairedDevice(this).ToList();
            newMapping.controller = controller;
            newMapping.role = role;
            newMapping.ready = true;
            return newMapping;
        }

        internal bool IsCompatibleWith(InputDevice device)
        {
            switch (controller)
            {
                case Controller.MouseKeyboard:
                    return device is Mouse or Keyboard;
                case Controller.Console:
                    return device is Gamepad;
                default:
                    return false;
            }
        }

        private bool IsOpenForPairingWith(InputDevice device)
        {
            if (!IsCompatibleWith(device))
                return false;
            IEnumerable<InputDevice> connectedDevices = inputMap.GetPairedDevice(this);
            switch (controller)
            {
                case Controller.MouseKeyboard: // decline pairing offer if existing keyboard/mouse already connected
                    if (device is Mouse)
                        return !connectedDevices.Any(connectedDevice => connectedDevice is Mouse);
                    return !connectedDevices.Any(connectedDevice => connectedDevice is Keyboard);
                case Controller.Console: // decline pairing offer if existing gamepad already connected
                    return !connectedDevices.Any(connectedDevice => connectedDevice is Gamepad);
                default:
                    return false;
            }
        }

        internal void UnpairFrom(InputDevice device)
        {
            if (!IsMappedToSelf(device))
                return;

            UnbindCallbackFromAction();
            inputMap.Pairings.Remove(device);
            OnDeviceMissing();
        }

        internal bool TryPairTo(InputDevice device)
        {
            if (inputMap.Pairings.TryAdd(device, this))
            {
                BindCallbackToAction();
                OnPairingSucceed();
                return true;
            }
            return false;
        }

        protected bool TryMapControllerTo(Role role)
        {
            ControllerMap mapping = inputMap.Mapping[role];
            if (mapping.Empty)
                return false;
            bool pairingSucceed = true;
            controller = mapping.controller;
            foreach (InputDevice device in mapping.pairedDevices)
            {
                if (!TryPairTo(device))
                    pairingSucceed = false;
            }
            return pairingSucceed;
        }

        protected abstract void MapSchemeTo(InputActionAsset action);
        protected abstract void BindCallbackToAction();
        protected abstract void UnbindCallbackFromAction();

        protected virtual void OnPairingSucceed() => action.Enable();

        protected virtual void OnDeviceMissing() { }

        protected bool IsMappedToSelf(CallbackContext ctx) =>
            IsMappedToSelf(ctx.control.FindInParentChain<InputDevice>());

        protected bool IsMappedToSelf(InputAction action)
        {
            if (action.activeControl == null)
                return false;
            return IsMappedToSelf(action.activeControl.FindInParentChain<InputDevice>());
        }

        protected bool IsMappedToSelf(InputDevice device)
        {
            if (inputMap == null)
                return false;
            return inputMap.Pairings.TryGetValue(device, out PlayerController controller)
                && controller == this;
        }

        protected void RequestPairing() => OnRequestPairing?.Invoke(this, this);

        private void RequestUnpair() => OnRequestUnpair?.Invoke(this, this);
    }
}
