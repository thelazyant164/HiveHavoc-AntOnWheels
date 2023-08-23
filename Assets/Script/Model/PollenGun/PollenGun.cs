using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    [Serializable]
    public sealed class PollenDictionary : AmmoDictionary { }

    public sealed class PollenGun : MonoBehaviour, IAltitudeAzimuthMount, ISwappableGun
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

        private void Awake()
        {
            ammoClip = GetComponent<PollenAmmoClip>();
        }

        private void Start()
        {
            Shooter shooter = PlayerManager.Instance.Shooter;
            shooter.OnAim += OnAim;
            shooter.OnShoot += OnShoot;

            Driver.Driver driver = PlayerManager.Instance.Driver;
            driver.OnReload += OnReload;
        }

        public void OnShoot(object sender, Ammo ammo)
        {
            if (
                ammoType.TryGetValue(ammo, out ISwappableShooter gun)
                && ammoClip.Ammo >= gun.AmmoCost
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

        public bool TryAimAz(Quaternion deltaAz) => true;

        public bool TryAimAlt(Quaternion deltaAlt)
        {
            Quaternion rot = altMount.localRotation;
            rot *= deltaAlt;
            return minAltitudeX < rot.eulerAngles.x;
        }
    }
}
