using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver
{
    public sealed class Driver : PlayerController
    {
        private static readonly Role role = Role.Driver;

        private InputAction throttle;
        private InputAction steer;
        private InputAction steerController;
        private InputAction brake;
        private InputAction reload;
        private InputAction pause;
        internal event EventHandler<float> OnAccelerate;
        internal event EventHandler<float> OnSteer;
        internal event EventHandler<bool> OnBrake;
        internal event EventHandler OnReload;
        internal event EventHandler OnPause;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            if (!TryMapControllerTo(role))
                RequestPairing();
        }

        private void Update()
        {
            if (steerController == null || !IsMappedToSelf(steerController))
                return;
            OnSteer?.Invoke(this, steerController.ReadValue<float>());
        }

        protected override void BindCallbackToAction()
        {
            throttle.started += Accelerate;
            steer.started += Steer;
            brake.started += Brake;
            reload.started += Reload;
            pause.started += Pause;
        }

        protected override void UnbindCallbackFromAction()
        {
            throttle.started -= Accelerate;
            steer.started -= Steer;
            brake.started -= Brake;
            reload.started -= Reload;
            pause.started -= Pause;
        }

        protected override void Pause()
        {
            throttle.started -= Accelerate;
            steer.started -= Steer;
            brake.started -= Brake;
            reload.started -= Reload;
        }

        protected override void Resume()
        {
            throttle.started += Accelerate;
            steer.started += Steer;
            brake.started += Brake;
            reload.started += Reload;
        }

        protected override void MapSchemeTo(InputActionAsset action)
        {
            InputActionMap driver = action.FindActionMap("Driver");
            throttle = driver.FindAction("Throttle");
            steer = driver.FindAction("Steer");
            steerController = driver.FindAction("SteerController");
            brake = driver.FindAction("Brake");
            reload = driver.FindAction("Reload");
            pause = driver.FindAction("Pause");
        }

        private void Accelerate(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            StartCoroutine(
                Move(throttle, (float inputValue) => OnAccelerate?.Invoke(this, inputValue))
            );
        }

        private void Steer(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            StartCoroutine(Move(steer, (float inputValue) => OnSteer?.Invoke(this, inputValue)));
        }

        private void Brake(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            StartCoroutine(ToggleBrake(brake, (bool active) => OnBrake?.Invoke(this, active)));
        }

        private IEnumerator Move(InputAction input, Action<float> callback)
        {
            while (!input.WasReleasedThisFrame())
            {
                callback(input.ReadValue<float>());
                yield return null;
            }
            callback(0);
        }

        private IEnumerator ToggleBrake(InputAction input, Action<bool> callback)
        {
            callback(true);
            while (input.IsPressed())
            {
                yield return null;
            }
            callback(false);
        }

        private void Reload(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            OnReload?.Invoke(this, EventArgs.Empty);
        }

        private void Pause(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            OnPause?.Invoke(this, EventArgs.Empty);
        }
    }
}
