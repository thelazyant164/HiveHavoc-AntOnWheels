using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Unnamed.RacingGame.Shooter
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

    public sealed class Cannon : MonoBehaviour
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

        [SerializeField]
        private Transform nozzle;

        [SerializeField]
        private GameObject cannonball;

        [SerializeField]
        private float launchForce = 300f;

        private Shooter shooter;

        private Vector3 AimDirection => (nozzle.position - spawn.position).normalized;

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
                Cannonball cannonball = Spawn();
                Launch(cannonball);
            };
        }

        private bool TryAimAlt(Quaternion deltaAlt)
        {
            Quaternion rot = altMount.localRotation;
            rot *= deltaAlt;
            return minAltitudeX < rot.eulerAngles.x;
        }

        private Cannonball Spawn() =>
            GameObject
                .Instantiate(cannonball, nozzle.position, Quaternion.identity)
                .GetComponent<Cannonball>();

        private void Launch(Cannonball cannonball) => cannonball.Launch(AimDirection * launchForce);
    }
}
