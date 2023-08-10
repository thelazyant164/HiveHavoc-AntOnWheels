using Com.Unnamed.RacingGame.Input;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.Unnamed.RacingGame.Shooter
{
    public sealed class Shooter : PlayerController
    {
        private static readonly Role role = Role.Shooter;

        private Camera shooterCamera;
        private Transform cameraAz;
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

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            if (!TryMapControllerTo(role))
            {
                RequestPairing();
            }
        }

        private void Update()
        {
            if (aimStick != null && IsMappedToSelf(aimStick))
            {
                Aim(aimStick.ReadValue<Vector2>() * joystickSensitivity * Time.deltaTime);
            }
        }

        protected override void BindCallbackToAction()
        {
            aimMouse.started += AimMouse;
            aimX.started += AimX;
            aimY.started += AimY;
            shoot.started += Shoot;
        }

        protected override void UnbindCallbackFromAction()
        {
            aimMouse.started -= AimMouse;
            aimX.started -= AimX;
            aimY.started -= AimY;
            shoot.started -= Shoot;
        }

        internal override void OnPairingSucceed()
        {
            base.OnPairingSucceed();
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected override void MapSchemeTo(InputActionAsset action)
        {
            InputActionMap shooter = action.FindActionMap("Shooter");
            aimStick = shooter.FindAction("AimStick");
            aimMouse = shooter.FindAction("AimMouse");
            aimX = shooter.FindAction("AimX");
            aimY = shooter.FindAction("AimY");
            shoot = shooter.FindAction("Shoot");
        }

        private void AimMouse(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            Aim(context.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime);
        }

        private void AimX(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            StartCoroutine(
                AimAxis(
                    aimX,
                    (float value) =>
                        Aim(new Vector2(value * keyPressSensitivity * Time.deltaTime, 0))
                )
            );
        }

        private void AimY(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            StartCoroutine(
                AimAxis(
                    aimY,
                    (float value) =>
                        Aim(
                            new Vector2(
                                0,
                                aimY.ReadValue<float>() * keyPressSensitivity * Time.deltaTime
                            )
                        )
                )
            );
        }

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
            if (
                minAzimuthY < cameraAz.localEulerAngles.y
                && cameraAz.localEulerAngles.y < maxAzimuthY
            )
            {
                cameraAz.localRotation *= Quaternion.Euler(0, -deltaMouseMovement.x, 0);
            }
            cameraAlt.localRotation *= Quaternion.Euler(-deltaMouseMovement.y, 0, 0);
            if (cameraAlt.localEulerAngles.x < maxAltitudeX)
            {
                cameraAlt.localRotation *= Quaternion.Euler(deltaMouseMovement.y, 0, 0);
            }
            OnAim?.Invoke(
                this,
                InputManager.GetRayToMouse(shooterCamera, InputManager.GetMouseScreenPosition())
            );
        }

        private void Shoot(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            OnShoot?.Invoke(this, EventArgs.Empty);
        }

        internal void SetupCamera(Transform az, Transform alt, Camera camera)
        {
            cameraAz = az;
            cameraAlt = alt;
            shooterCamera = camera;
        }
    }
}
