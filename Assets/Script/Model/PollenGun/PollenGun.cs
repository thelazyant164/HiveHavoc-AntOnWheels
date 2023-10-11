using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using System;
using UnityEngine;

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
        [SerializeField]
        private float minAltitudeX = 320;

        [Space]
        [Header("Ammo types")]
        [SerializeField]
        private PollenDictionary ammoType = new();
        public AmmoDictionary AmmoType => ammoType;

        private PollenAmmoClip ammoClip;
        public IDepletableAmmo Ammo => ammoClip;

        private PollenShooter shooter;

        [Space]
        [Header("Aim crosshair")]
        [SerializeField]
        private float maxAimDistance;
        public float MaxAimDistance => maxAimDistance;

        [SerializeField]
        private LayerMask aimInterest;
        public LayerMask AimInterest => aimInterest;

        public event EventHandler<AimTarget> OnAimTargetChange;

        private void Awake()
        {
            ammoClip = GetComponent<PollenAmmoClip>();
            shooter = GetComponent<PollenShooter>();
        }

        private void Start()
        {
            Register(GameManager.Instance);

            Shooter shooter = PlayerManager.Instance.Shooter;
            shooter.OnAim += OnAim;
            shooter.OnShoot += OnShoot;

            Driver.Driver driver = PlayerManager.Instance.Driver;
            driver.OnReload += OnReload;
        }

        private void Update()
        {
            TryGetAimTarget(out AimTarget target);
            OnAimTargetChange?.Invoke(this, target);
        }

        public void Register(IServiceProvider<PollenGun> provider) => provider.Register(this);

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
        }

        private void OnReload(object sender, EventArgs e) => ammoClip.Reload();

        public void OnAim(object sender, AimDelta delta)
        {
            if (TryAimAz(delta.az))
                azMount.localRotation *= delta.az;
            if (TryAimAlt(delta.alt))
                altMount.localRotation *= delta.alt;
        }

        public bool TryGetAimTarget(out AimTarget result)
        {
            result = AimTarget.None;
            Ray ray = new Ray(shooter.ProjectileSpawn, shooter.AimDirection);
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
            return minAltitudeX < rot.eulerAngles.x;
        }
    }
}
