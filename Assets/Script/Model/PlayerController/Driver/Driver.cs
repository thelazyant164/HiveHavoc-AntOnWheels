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
        internal event EventHandler<float> OnAccelerate;
        internal event EventHandler<float> OnSteer;

        protected override void BindCallbackToAction()
        {
            throttle.started += Accelerate;
            steer.started += Steer;
        }

        protected override void UnbindCallbackFromAction()
        {
            throttle.started -= Accelerate;
            steer.started -= Steer;
        }

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

        protected override void MapSchemeTo(InputActionAsset action)
        {
            InputActionMap driver = action.FindActionMap("Driver");
            throttle = driver.FindAction("Throttle");
            steer = driver.FindAction("Steer");
            movementController = driver.FindAction("MovementController");
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

        private IEnumerator Move(InputAction input, Action<float> callback)
        {
            while (input.IsPressed())
            {
                callback(input.ReadValue<float>());
                yield return null;
            }
            callback(0);
        }
    }
}
