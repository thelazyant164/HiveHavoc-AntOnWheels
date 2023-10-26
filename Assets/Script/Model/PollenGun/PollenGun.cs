using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    [Serializable]
    public sealed class PollenDictionary : AmmoDictionary { }

    public sealed class PollenGun
        : MonoBehaviour,
            IAltitudeAzimuthMount,
            ISwappableGun,
            IService<PollenGun>
    {
        [Header("Az-alt mount")]
        [SerializeField]
        private Transform azMount;
        public Transform Azimuth => azMount;

        [SerializeField]
        private Transform altMount;
        public Transform Altitude => altMount;

        [Space]
        [Header("Constraint")]
        [SerializeField, Range(-90, 0)]
        private float minAngle = -30;

        [SerializeField, Range(0, 90)]
        private float maxAngle = 30;

        [Space]
        [Header("Ammo types")]
        [SerializeField]
        private PollenDictionary ammoType = new();
        public AmmoDictionary AmmoType => ammoType;

        private PollenAmmoClip ammoClip;
        public IDepletableAmmo Ammo => ammoClip;

        private PollenShooter shooterComponent;

        [Space]
        [Header("Aim crosshair")]
        [SerializeField]
        private float maxAimDistance;
        public float MaxAimDistance => maxAimDistance;

        [SerializeField]
        private LayerMask aimInterest;
        public LayerMask AimInterest => aimInterest;

        [Space]
        [Header("SFX")]
        [SerializeField]
        private AudioSource ammoAudio;

        [SerializeField]
        private AudioClip blankSFX;

        private Shooter shooter;

        public event EventHandler<AimTarget> OnAimTargetChange;

        private void Awake()
        {
            ammoClip = GetComponent<PollenAmmoClip>();
            shooterComponent = GetComponent<PollenShooter>();
        }

        private void Start()
        {
            Register(ServiceManager.Instance.AimService);

            shooter = PlayerManager.Instance.Shooter;
            shooter.OnAim += OnAim;
            shooter.OnShoot += OnShoot;

            Driver.Driver driver = PlayerManager.Instance.Driver;
            driver.OnReload += OnReload;

            // reset rotation after constraint applied to ensure within range
            azMount.localRotation = Quaternion.identity;
            altMount.localRotation = Quaternion.identity;
        }

        private void Update()
        {
            TryGetAimTarget(out AimTarget target);
            OnAimTargetChange?.Invoke(this, target);
        }

        public void Register(IServiceProvider<PollenGun> provider) => provider.Register(this);

        /// <summary>
        /// Override gun aim controls for a short duration.
        /// </summary>
        /// <remarks>
        /// Used to pivot turret to point of interest before detach camera.
        /// </remarks>
        /// <param name="position">Point of interest.</param>
        /// <param name="duration">Time to lerp to point of interest. Expected to have pointed at point of interest by the time this runs out.</param>
        internal void LookAt(Vector3 position, float duration)
        {
            shooter.OnAim -= OnAim; // temporarily disable player input-driven aiming

            StartCoroutine(LookAtAz(position, duration));
            StartCoroutine(LookAtAlt(position, duration));

            gameObject.SetTimeOut(duration, () => shooter.OnAim += OnAim); // enable player input-driven aiming again
        }

        private IEnumerator LookAtAz(Vector3 target, float duration)
        {
            Quaternion targetAz = Quaternion.LookRotation(target - azMount.position, Vector3.up);
            Quaternion deltaAz = targetAz * Quaternion.Inverse(azMount.rotation);
            float deltaYaw = deltaAz.eulerAngles.y;

            Quaternion currentRot = azMount.localRotation;
            Quaternion addedRot = Quaternion.AngleAxis(deltaYaw, azMount.up);
            Quaternion targetRot = currentRot * addedRot;
            float timeRemaining = duration;
            while (timeRemaining > 0)
            {
                azMount.localRotation = Quaternion.Lerp(
                    currentRot,
                    targetRot,
                    (duration - timeRemaining) / duration
                );
                timeRemaining -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator LookAtAlt(Vector3 target, float duration)
        {
            Quaternion targetAlt = Quaternion.LookRotation(target - altMount.position, Vector3.up);
            Quaternion deltaAlt = targetAlt * Quaternion.Inverse(altMount.rotation);
            float deltaPitch = deltaAlt.eulerAngles.x;

            Quaternion currentRot = altMount.localRotation;
            Quaternion addedRot = Quaternion.AngleAxis(deltaPitch, azMount.right);
            Quaternion targetRot = currentRot * addedRot;
            targetRot = Quaternion.Euler(
                ClampOriginalRotationX(targetRot, minAngle, maxAngle),
                0,
                0
            );
            float timeRemaining = duration;
            while (timeRemaining > 0)
            {
                altMount.localRotation = Quaternion.Lerp(
                    currentRot,
                    targetRot,
                    (duration - timeRemaining) / duration
                );
                timeRemaining -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        public void OnShoot(object sender, Ammo ammo)
        {
            if (
                ammoType.TryGetValue(ammo, out ISwappableShooter gun)
                && ammoClip.Ammo >= gun.AmmoCost
                && (gun as PollenShooter).Ready
            )
            {
                ammoClip.Consume(gun.AmmoCost);
                gun.Shoot();
            }
            else
            {
                ammoAudio.PlayOneShot(blankSFX);
            }
        }

        private void OnReload(object sender, EventArgs e) => ammoClip.Reload();

        public void OnAim(object sender, AimDelta delta)
        {
            if (TryAimAz(delta.az))
                azMount.localRotation *= delta.az;
            if (TryAimAlt(delta.alt))
            {
                altMount.localRotation *= delta.alt;
            }
            else
            {
                altMount.SetLocalPositionAndRotation(
                    altMount.localPosition,
                    Quaternion.Euler(
                        ClampOriginalRotationX(altMount.localRotation, minAngle, maxAngle),
                        altMount.localRotation.y,
                        altMount.localRotation.z
                    )
                );
            }
        }

        public bool TryGetAimTarget(out AimTarget result)
        {
            result = AimTarget.None;
            Ray ray = new Ray(shooterComponent.ProjectileSpawn, shooterComponent.AimDirection);
            // Debug.DrawLine(ray.origin, ray.GetPoint(maxAimDistance), Color.red);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, maxAimDistance, aimInterest))
            {
                // Debug.Log($"Aim hit {raycastHit.transform.gameObject}");
                result = AimTarget.ObjectOfInterest;
            }
            return result != AimTarget.None;
        }

        public bool TryAimAz(Quaternion deltaAz) => true;

        public bool TryAimAlt(Quaternion deltaAlt)
        {
            Quaternion rot = altMount.localRotation;
            rot *= deltaAlt;

            float currentRot = GetNormalizedRotationX(rot);
            return minAngle <= currentRot && currentRot <= maxAngle;
        }

        private float ClampOriginalRotationX(
            Quaternion quat,
            float minNormalized,
            float maxNormalized
        )
        {
            float normalizedX = GetNormalizedRotationX(quat);
            float clampedNormalized = ClampNormalizedRotationX(
                normalizedX,
                minNormalized,
                maxNormalized
            );
            float original = GetOriginalRotationX(clampedNormalized);
            return original;
        }

        private float GetNormalizedRotationX(Quaternion quat) =>
            quat.eulerAngles.x >= 180 ? 360 - quat.eulerAngles.x : -quat.eulerAngles.x;

        private float ClampNormalizedRotationX(float normalizedX, float minAngle, float maxAngle) =>
            Mathf.Clamp(normalizedX, minAngle, maxAngle);

        private float GetOriginalRotationX(float normalizedX) =>
            normalizedX >= 0 ? 360 - normalizedX : -normalizedX;
    }
}
