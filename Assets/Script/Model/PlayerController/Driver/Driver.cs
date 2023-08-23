using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private InputAction movementController;
        private InputAction brake;
        private InputAction reload;
        internal event EventHandler<float> OnAccelerate;
        internal event EventHandler<float> OnSteer;
        internal event EventHandler<bool> OnBrake;
        internal event EventHandler OnReload;

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
            if (movementController == null || !IsMappedToSelf(movementController))
                return;
            Vector2 inputValue = movementController.ReadValue<Vector2>();
            OnSteer?.Invoke(this, inputValue.x);
            OnAccelerate?.Invoke(this, inputValue.y);
        }

        protected override void BindCallbackToAction()
        {
            throttle.started += Accelerate;
            steer.started += Steer;
            brake.started += Brake;
            reload.started += Reload;
        }

        protected override void UnbindCallbackFromAction()
        {
            throttle.started -= Accelerate;
            steer.started -= Steer;
            brake.started -= Brake;
            reload.started -= Reload;
        }

        protected override void MapSchemeTo(InputActionAsset action)
        {
            InputActionMap driver = action.FindActionMap("Driver");
            throttle = driver.FindAction("Throttle");
            steer = driver.FindAction("Steer");
            movementController = driver.FindAction("MovementController");
            brake = driver.FindAction("Brake");
            reload = driver.FindAction("Reload");
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
            while (input.IsPressed())
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

        private void Reload(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            OnReload?.Invoke(this, EventArgs.Empty);
        }
    }
}
