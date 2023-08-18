using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public struct AimDelta
    {
        public Quaternion az;
        public Quaternion alt;

        public AimDelta(Quaternion az, Quaternion alt)
        {
            this.az = az;
            this.alt = alt;
        }
    }

    public sealed class Cannon : MonoBehaviour, IShootable<Cannonball>
    {
        [Header("Az-alt mount")]
        [SerializeField]
        private Transform azMount;

        [SerializeField]
        private Transform altMount;

        [Space]
        [Header("Constraint")]
        [SerializeField]
        private float minAltitudeX = 320;

        [Space]
        [Header("Cannonball spawn")]
        [SerializeField]
        private Transform spawn;
        public Vector3 ProjectileSpawnPosition => spawn.position;

        [SerializeField]
        private Transform nozzle;

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

        public event EventHandler<Cannonball> OnShoot;

        private void Start()
        {
            shooter = PlayerManager.Instance.Shooter;
            shooter.OnAim += (object sender, AimDelta delta) =>
            {
                azMount.localRotation *= delta.az;
                if (TryAimAlt(delta.alt))
                    altMount.localRotation *= delta.alt;
            };
            shooter.OnShoot += (object sender, EventArgs e) =>
            {
                if (!Ready)
                    return;
                Ready = false;
                Cannonball cannonball = SpawnProjectile();
                Launch(cannonball);
                OnShoot?.Invoke(this, cannonball);
                gameObject.SetTimeOut(cooldownDuration, () => Ready = true);
            };
        }

        private bool TryAimAlt(Quaternion deltaAlt)
        {
            Quaternion rot = altMount.localRotation;
            rot *= deltaAlt;
            return minAltitudeX < rot.eulerAngles.x;
        }

        public Cannonball SpawnProjectile() =>
            GameObject
                .Instantiate(cannonball, nozzle.position, Quaternion.identity)
                .GetComponent<Cannonball>();

        public void Launch(Cannonball cannonball) =>
            cannonball.Launch(AimDirection * initialImpulse);
    }
}
