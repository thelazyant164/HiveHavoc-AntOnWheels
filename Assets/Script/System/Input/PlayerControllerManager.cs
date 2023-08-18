using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input
{
    [Serializable]
    public sealed class InputMap : EventArgs
    {
        [SerializeField]
        private List<ControllerMap> mappings;
        public IEnumerable<ControllerMap> Mappings
        {
            get => mappings;
            private set => mappings = value.ToList();
        }

        public InputMap(IEnumerable<ControllerMap> controllers) => Mappings = controllers;

        public ControllerMap this[Role key]
        {
            get => Mappings.ToList().Find(mapping => mapping.role == key);
        }
    }

    public sealed class PlayerControllerManager : Singleton<PlayerControllerManager>
    {
        [SerializeField]
        private InputMap mapping;
        public InputMap Mapping
        {
            get => mapping;
            private set => mapping = value;
        }
        public HashSet<InputDevice> Devices { get; private set; } = new();
        public Dictionary<InputDevice, PlayerController> Pairings { get; private set; } = new();
        public event EventHandler<InputDevice> OnDeviceAdd;
        public event EventHandler<InputDevice> OnDeviceRemove;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            Devices = InputSystem.devices.ToHashSet();
            InputSystem.onDeviceChange += ManageDeviceList;
            PlayerController.OnRequestPairing += (object sender, PlayerController controller) =>
                AttemptPairing(controller);
            PlayerController.OnRequestUnpair += (object sender, PlayerController controller) =>
            {
                InputActionAsset action = controller.Action;
                foreach (InputDevice device in Devices)
                {
                    controller.UnpairFrom(device);
                }
                if (!IsBeingUsed(action))
                    action.Disable();
            };
        }

        private void Start()
        {
            IndicatorManager indicatorManager = IndicatorManager.Instance;
            if (indicatorManager == null)
                return;

            indicatorManager.OnReady += (object sender, InputMap inputMap) => Mapping = inputMap;
        }

        private void ManageDeviceList(InputDevice device, InputDeviceChange changeEvent)
        {
            switch (changeEvent)
            {
                case InputDeviceChange.Added
                or InputDeviceChange.Enabled
                or InputDeviceChange.Reconnected:
                    if (Devices.Add(device))
                        OnDeviceAdd?.Invoke(this, device);
                    break;
                case InputDeviceChange.Removed
                or InputDeviceChange.Disabled
                or InputDeviceChange.Disconnected:
                    if (Devices.Remove(device))
                        OnDeviceRemove?.Invoke(this, device);
                    break;
                default:
                    break;
            }
        }

        private bool IsAvailable(InputDevice device) => !Pairings.TryGetValue(device, out var pair);

        private void AttemptPairing(PlayerController controller)
        {
            foreach (InputDevice device in Devices)
            {
                if (IsAvailable(device) && controller.IsCompatibleWith(device))
                    controller.TryPairTo(device);
            }
        }

        private bool IsBeingUsed(InputActionAsset action) =>
            Pairings.Values.Any(controller => controller.Action == action);

        internal IEnumerable<InputDevice> GetPairedDevice(PlayerController controller) =>
            Pairings.Where(pairing => pairing.Value == controller).Select(pairing => pairing.Key);
    }
}
