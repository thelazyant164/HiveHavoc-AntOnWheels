using Com.Unnamed.RacingGame.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Unnamed.RacingGame.Shooter
{
    public sealed class Cannon
        : MonoBehaviour
    {
        [Header("Aim target")]
        [SerializeField]
        private Transform aimAz;
        [SerializeField]
        private Transform aimAlt;
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
        private SphereCollider aimSphere;

        private Vector3 AimDirection => (nozzle.position - spawn.position).normalized;

        private void Awake()
        {
            aimSphere = GetComponentInChildren<SphereCollider>();
        }

        private void Start()
        {
            shooter = PlayerManager.Instance.Shooter;
            shooter.OnAim += (object sender, Ray forward) =>
            {
                if (TryAim(forward, out Vector3 target))
                {
                    aimAz.position = target;
                    aimAlt.position = target;
                }
            };
            shooter.OnShoot += (object sender, EventArgs e) =>
            {
                Cannonball cannonball = Spawn();
                Launch(cannonball);
            };
        }

        private bool TryAim(Ray forward, out Vector3 result)
        {
            result = Vector3.zero;
            if (aimSphere.Raycast(ReverseRay(forward), out RaycastHit raycastHit, float.MaxValue))
            {
                result = raycastHit.point;
            }
            return result != Vector3.zero;
        }

        // Reverse ray, due to Unity's default behaviour of ignoring ray hit when source originates from inside collision volume
        private Ray ReverseRay(Ray ray)
        {
            ray.origin = ray.GetPoint(aimSphere.radius * 2);
            ray.direction = -ray.direction;
            return ray;
        }

        private Cannonball Spawn() => GameObject.Instantiate(cannonball, nozzle.position, Quaternion.identity).GetComponent<Cannonball>();

        private void Launch(Cannonball cannonball) => cannonball.Launch(AimDirection * launchForce);
    }
}
