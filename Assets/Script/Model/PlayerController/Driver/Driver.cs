using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Com.Unnamed.RacingGame.Driver
{
    public sealed class Driver : MonoBehaviour
    {
        private InputAction throttle;
        private InputAction steer;
        private InputAction movementController;
        internal event EventHandler<float> OnAccelerate;
        internal event EventHandler<float> OnSteer;

        private void OnEnable()
        {
            throttle.started += Accelerate;
            steer.started += Steer;
        }

        private void OnDisable()
        {
            throttle.started -= Accelerate;
            steer.started -= Steer;
        }

        private void Awake()
        {
            InputActionMap driver = GetComponent<PlayerInput>().actions.FindActionMap("Driver");
            throttle = driver.FindAction("Throttle");
            steer = driver.FindAction("Steer");
            movementController = driver.FindAction("MovementController");
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (movementController == null)
                return;
            Vector2 inputValue = movementController.ReadValue<Vector2>();
            OnSteer?.Invoke(this, inputValue.x);
            OnAccelerate?.Invoke(this, inputValue.y);
        }

        private void Accelerate(CallbackContext context) =>
            StartCoroutine(
                Move(throttle, (float inputValue) => OnAccelerate?.Invoke(this, inputValue))
            );

        private void Steer(CallbackContext context) =>
            StartCoroutine(Move(steer, (float inputValue) => OnSteer?.Invoke(this, inputValue)));

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
