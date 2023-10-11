using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Setting;
using static UnityEngine.InputSystem.InputAction;
using static Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Setting.Setting;

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
        private InputAction pause;

        private float currentThruster;
        private float thrusterBurnout;
        internal event EventHandler<AimDelta> OnAim;
        internal event EventHandler<Ammo> OnShoot;
        internal event EventHandler<float> OnThruster;
        internal event EventHandler OnPause;

        protected override void Awake()
        {
            base.Awake();
            BindToSetting();
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
                Aim(aimStick.ReadValue<Vector2>() * joystickSensitivity * Time.unscaledDeltaTime);
            }
        }

        private void BindToSetting()
        {
            SettingManager settings = SettingManager.Instance;
            if (settings == null || !settings.IsOverriden)
                return;

            settings[MouseAimSensitivity].OnChange += (object sender, float value) =>
                mouseSensitivity = value;
            settings[KeyboardAimSensitivity].OnChange += (object sender, float value) =>
                keyPressSensitivity = value;
            settings[ConsoleAimSensitivity].OnChange += (object sender, float value) =>
                joystickSensitivity = value;
            settings[ConsoleSpamSensitivity].OnChange += (object sender, float value) =>
                controllerSensitivity = value;
        }

        protected override void BindCallbackToAction()
        {
            aimMouse.started += AimMouse;
            aimX.started += AimX;
            aimY.started += AimY;
            shootPrimary.started += ShootPrimary;
            shootSecondary.started += ShootSecondary;
            thruster.started += Thruster;
            pause.started += Pause;

            StartCoroutine(ThrusterDeteriorate());
        }

        protected override void UnbindCallbackFromAction()
        {
            ResetThruster();

            aimMouse.started -= AimMouse;
            aimX.started -= AimX;
            aimY.started -= AimY;
            shootPrimary.started -= ShootPrimary;
            shootSecondary.started -= ShootSecondary;
            thruster.started -= Thruster;
            pause.started -= Pause;
        }

        protected override void Pause()
        {
            ResetThruster();

            aimMouse.started -= AimMouse;
            aimX.started -= AimX;
            aimY.started -= AimY;
            shootPrimary.started -= ShootPrimary;
            shootSecondary.started -= ShootSecondary;
            thruster.started -= Thruster;
        }

        protected override void Resume()
        {
            aimMouse.started += AimMouse;
            aimX.started += AimX;
            aimY.started += AimY;
            shootPrimary.started += ShootPrimary;
            shootSecondary.started += ShootSecondary;
            thruster.started += Thruster;
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
            pause = shooter.FindAction("Pause");
        }

        private void AimMouse(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            Aim(context.ReadValue<Vector2>() * mouseSensitivity * Time.unscaledDeltaTime);
        }

        private void AimX(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            StartCoroutine(
                AimAxis(
                    aimX,
                    (float value) =>
                        Aim(new Vector2(value * keyPressSensitivity * Time.unscaledDeltaTime, 0))
                )
            );
        }

        private void AimY(CallbackContext context)
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
                                aimY.ReadValue<float>()
                                    * keyPressSensitivity
                                    * Time.unscaledDeltaTime
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

        private void ShootPrimary(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            OnShoot?.Invoke(this, Ammo.Primary);
        }

        private void ShootSecondary(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            OnShoot?.Invoke(this, Ammo.Secondary);
        }

        private void Thruster(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            float value = context.ReadValue<float>();
            value = Controller is Controller.Console ? value * controllerSensitivity : value;
            currentThruster =
                Mathf.Sign(value) != Mathf.Sign(currentThruster) ? value : currentThruster;
        }

        private void Pause(CallbackContext context)
        {
            if (!IsMappedToSelf(context))
                return;
            OnPause?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerator ThrusterDeteriorate()
        {
            thrusterBurnout = 1;
            float sign;
            while (true)
            {
                sign = Mathf.Sign(currentThruster);
                thrusterBurnout += Time.fixedDeltaTime;
                currentThruster -=
                    Time.fixedDeltaTime * thrusterBurnout * Mathf.Sign(currentThruster);
                if (sign != Mathf.Sign(currentThruster))
                {
                    ResetThruster();
                }
                OnThruster?.Invoke(this, Mathf.Abs(currentThruster));
                yield return new WaitForFixedUpdate();
            }
        }

        private void ResetThruster()
        {
            currentThruster = 0;
            thrusterBurnout = 1;
            OnThruster?.Invoke(this, 0);
        }
    }
}
