using Com.Unnamed.RacingGame.Input;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.Unnamed.RacingGame.Shooter
{
    public sealed class Shooter : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField]
        private Camera shooterCamera;
        [SerializeField]
        private Transform cameraAz;
        [SerializeField]
        private Transform cameraAlt;
        [Space]

        [Header("Constraint")]
        [SerializeField]
        private float minAzimuthY = 90;
        [SerializeField]
        private float maxAzimuthY = 270;
        [SerializeField]
        private float maxAltitudeX = 300;
        [Space]

        [Header("Input")]
        [SerializeField, Range(10f, 20f)]
        private float mouseSensitivity = 10f;
        [SerializeField, Range(70f, 100f)]
        private float keyPressSensitivity = 70f;
        [SerializeField, Range(120f, 150f)]
        private float joystickSensitivity = 120f;

        private InputAction aimStick;
        private InputAction aimMouse;
        private InputAction aimX;
        private InputAction aimY;
        private InputAction shoot;
        internal event EventHandler<Ray> OnAim;
        internal event EventHandler OnShoot;

        private void Awake()
        {
            InputActionMap shooter = GetComponent<PlayerInput>().actions.FindActionMap("Shooter");
            aimStick = shooter.FindAction("AimStick");
            aimMouse = shooter.FindAction("AimMouse");
            aimX = shooter.FindAction("AimX");
            aimY = shooter.FindAction("AimY");
            shoot = shooter.FindAction("Shoot");
        }

        private void OnEnable()
        {
            aimMouse.started += AimMouse;
            aimX.started += AimX;
            aimY.started += AimY;
            shoot.started += Shoot;
        }

        private void OnDisable()
        {
            aimMouse.started -= AimMouse;
            aimX.started -= AimX;
            aimY.started -= AimY;
            shoot.started -= Shoot;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (aimStick != null)
            {
                Aim(aimStick.ReadValue<Vector2>() * joystickSensitivity * Time.deltaTime);
            }
        }

        private void AimMouse(InputAction.CallbackContext context) => Aim(context.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime);

        private void AimX(InputAction.CallbackContext context) => StartCoroutine(AimAxis(aimX, (float value) => Aim(new Vector2(value * keyPressSensitivity * Time.deltaTime, 0))));

        private void AimY(InputAction.CallbackContext context) => StartCoroutine(AimAxis(aimY, (float value) => Aim(new Vector2(0, aimY.ReadValue<float>() * keyPressSensitivity * Time.deltaTime))));

        private IEnumerator AimAxis(InputAction input, Action<float> callback)
        {
            while (input.IsPressed())
            {
                callback(input.ReadValue<float>());
                yield return null;
            }
        }

        private void Aim(Vector2 deltaMouseMovement)
        {
            cameraAz.localRotation *= Quaternion.Euler(0, deltaMouseMovement.x, 0);
            if (minAzimuthY < cameraAz.localEulerAngles.y && cameraAz.localEulerAngles.y < maxAzimuthY)
            {
                cameraAz.localRotation *= Quaternion.Euler(0, -deltaMouseMovement.x, 0);
            }
            cameraAlt.localRotation *= Quaternion.Euler(-deltaMouseMovement.y, 0, 0);
            if (cameraAlt.localEulerAngles.x < maxAltitudeX)
            {
                cameraAlt.localRotation *= Quaternion.Euler(deltaMouseMovement.y, 0, 0);
            }
            OnAim?.Invoke(this, InputManager.GetRayToMouse(shooterCamera, InputManager.GetMouseScreenPosition()));
        }

        private void Shoot(InputAction.CallbackContext context) => OnShoot?.Invoke(this, EventArgs.Empty);
    }
}
