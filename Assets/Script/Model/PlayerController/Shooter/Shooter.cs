using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public enum Ammo
    {
        Primary,
        Secondary
    }

    public sealed class Shooter : PlayerController
    {
        private static readonly Role role = Role.Shooter;

        [Space]
        [Header("Sensitivity")]
        [SerializeField, Range(10f, 20f)]
        private float mouseSensitivity = 10f;

        [SerializeField, Range(70f, 100f)]
        private float keyPressSensitivity = 70f;

        [SerializeField, Range(120f, 150f)]
        private float joystickSensitivity = 120f;

        [SerializeField, Range(1.15f, 1.2f)]
        private float controllerSensitivity = 1.18f;

        private InputAction aimStick;
        private InputAction aimMouse;
        private InputAction aimX;
        private InputAction aimY;
        private InputAction shootPrimary;
        private InputAction shootSecondary;
        private InputAction thruster;
        private float currentThruster;
        internal event EventHandler<AimDelta> OnAim;
        internal event EventHandler<Ammo> OnShoot;
        internal event EventHandler<float> OnThruster;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(ThrusterDeteriorate());
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
            shootPrimary.started += ShootPrimary;
            shootSecondary.started += ShootSecondary;
            thruster.started += Thruster;
        }

        protected override void UnbindCallbackFromAction()
        {
            aimMouse.started -= AimMouse;
            aimX.started -= AimX;
            aimY.started -= AimY;
            shootPrimary.started -= ShootPrimary;
            shootSecondary.started -= ShootSecondary;
            thruster.started -= Thruster;
        }

        protected override void OnPairingSucceed()
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
            shootPrimary = shooter.FindAction("ShootPrimary");
            shootSecondary = shooter.FindAction("ShootSecondary");
            thruster = shooter.FindAction("Thruster");
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

        private void Aim(Vector2 deltaMouseMovement) =>
            OnAim?.Invoke(
                this,
                new AimDelta(
                    Quaternion.Euler(0, deltaMouseMovement.x, 0),
                    Quaternion.Euler(-deltaMouseMovement.y, 0, 0)
                )
            );

        private void ShootPrimary(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            OnShoot?.Invoke(this, Ammo.Primary);
        }

        private void ShootSecondary(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            OnShoot?.Invoke(this, Ammo.Secondary);
        }

        private void Thruster(InputAction.CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            float value = context.ReadValue<float>();
            value = Controller is Controller.Console ? value * controllerSensitivity : value;
            currentThruster =
                Mathf.Sign(value) != Mathf.Sign(currentThruster) ? value : currentThruster;
        }

        private IEnumerator ThrusterDeteriorate()
        {
            float timeElapsed = 1;
            float sign;
            while (true)
            {
                sign = Mathf.Sign(currentThruster);
                timeElapsed += Time.deltaTime;
                currentThruster -= Time.deltaTime * timeElapsed * Mathf.Sign(currentThruster);
                if (sign != Mathf.Sign(currentThruster))
                {
                    currentThruster = 0;
                    timeElapsed = 1;
                }
                OnThruster?.Invoke(this, Mathf.Abs(currentThruster));
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
