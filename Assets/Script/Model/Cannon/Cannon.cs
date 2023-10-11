using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class Cannon : MonoBehaviour, IAltitudeAzimuthMount, IShootable<Cannonball>
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
        [Header("Cannonball spawn")]
        [SerializeField]
        private Transform spawn;
        public Vector3 ProjectileSpawn => spawn.position;

        [SerializeField]
        private Transform nozzle;
        public Vector3 Nozzle => nozzle.position;

        [SerializeField]
        private GameObject cannonball;
        public Cannonball Projectile => cannonball.GetComponent<Cannonball>();

        [SerializeField]
        private float initialImpulse = 300f;
        public float InitialImpulse => initialImpulse;

        [SerializeField]
        private float cooldownDuration;
        public float CooldownDuration => cooldownDuration;
        public bool Ready { get; private set; } = true;

        private Shooter shooter;
        public Vector3 AimDirection => (nozzle.position - spawn.position).normalized;

        [Space]
        [Header("Aim crosshair")]
        [SerializeField]
        private float maxAimDistance;
        public float MaxAimDistance => maxAimDistance;

        [SerializeField]
        private LayerMask aimInterest;
        public LayerMask AimInterest => aimInterest;

        public event EventHandler<Cannonball> OnShoot;
        public event EventHandler<AimTarget> OnAimTargetChange;

        private void Start()
        {
            OnAimTargetChange?.Invoke(this, AimTarget.None); // silence warning

            shooter = PlayerManager.Instance.Shooter;
            shooter.OnAim += OnAim;
            shooter.OnShoot += (object sender, Ammo ammo) => Shoot();
        }

        public bool TryAimAz(Quaternion deltaAz) => true;

        public bool TryAimAlt(Quaternion deltaAlt)
        {
            Quaternion rot = altMount.localRotation;
            rot *= deltaAlt;
            return minAltitudeX < rot.eulerAngles.x;
        }

        public void OnAim(object sender, AimDelta delta)
        {
            if (TryAimAz(delta.az))
                azMount.localRotation *= delta.az;
            if (TryAimAlt(delta.alt))
                altMount.localRotation *= delta.alt;
        }

        public Cannonball SpawnProjectile() =>
            GameObject
                .Instantiate(cannonball, nozzle.position, Quaternion.identity)
                .GetComponent<Cannonball>();

        public void Launch(Cannonball cannonball) =>
            cannonball.Launch(AimDirection * initialImpulse);

        public void Shoot()
        {
            if (!Ready)
                return;
            Ready = false;
            Cannonball cannonball = SpawnProjectile();
            Launch(cannonball);
            OnShoot?.Invoke(this, cannonball);
            gameObject.SetTimeOut(cooldownDuration, () => Ready = true);
        }

        public bool TryGetAimTarget(out AimTarget result)
        {
            result = AimTarget.None;
            return false;
        }
    }
}
