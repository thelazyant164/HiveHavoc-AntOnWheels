using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy
{
    [RequireComponent(typeof(Collider))]
    public sealed class TrapShooter
        : MonoBehaviour,
            IShootable<EnemyProjectile>,
            IDestructible<TrapShooter>
    {
        [Header("Trigger")]
        [SerializeField]
        private List<TrapTrigger> triggers;

        [SerializeField]
        private Transform target;
        private GameObject cachedTarget;

        [Space]
        [Header("Ammo")]
        [SerializeField]
        private EnemyProjectile projectile;
        public EnemyProjectile Projectile => projectile;

        [SerializeField]
        private float initialImpulse;
        public float InitialImpulse => initialImpulse;

        public Vector3 ProjectileSpawn => torpedoTrajectory.sample[TrajectoryPoint.Origin];

        public Vector3 Nozzle => torpedoTrajectory.sample[TrajectoryPoint.Tip];
        public Vector3 AimDirection => (Nozzle - ProjectileSpawn).normalized;

        public float CooldownDuration => 0;
        public bool Ready => true;

        [SerializeField]
        private LayerMask destroyedBy;
        public LayerMask DestroyedBy => destroyedBy;

        private TrapTrigger activator;
        private Trajectory torpedoTrajectory;

        [SerializeField]
        private BeeAnimation bee;

        private event EventHandler OnActivate;
        public event EventHandler<EnemyProjectile> OnShoot;
        public event EventHandler<TrapShooter> OnDestroy;

        private void Awake()
        {
            if (triggers.Count == 0)
                return;
            cachedTarget = target.gameObject;
            bee.OnEstablish += (object sender, Trajectory trajectory) =>
            {
                torpedoTrajectory = trajectory;
                Shoot();
            };

            OnActivate += Activate;
            foreach (TrapTrigger trigger in triggers)
            {
                trigger.OnTrigger += (object sender, EventArgs e) => OnActivate?.Invoke(sender, e);
                OnShoot += (object sender, EnemyProjectile projectile) =>
                {
                    if ((TrapTrigger)sender != trigger)
                        return;
                    trigger.InvokeOnTerminate();
                };
            }
        }

        private void Start()
        {
            if (target.TryGetComponent(out IRespawnable respawnable))
            {
                RespawnManager.Instance.OnRespawn += (object sender, GameObject respawned) =>
                {
                    if (sender == (object)cachedTarget)
                    {
                        target = respawned.transform;
                    }
                };
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.InLayerMask(destroyedBy))
            {
                Destroy();
            }
        }

        private void Activate(object sender, EventArgs e)
        {
            gameObject.SetActive(true);
            bee.PlayShoot();
            //OnActivate -= Activate; // TODO: solution to handle 2 trigger activating the same shooter; desired behaviour: when 1 trigger, other cannot trigger - current behaviour: both can trigger, unless shooter destroyed
            activator = (TrapTrigger)sender;
        }

        public void Shoot()
        {
            GameObject projectileGO = Instantiate(
                torpedoTrajectory.kinematicProjectile,
                torpedoTrajectory.sample[TrajectoryPoint.Tip],
                Quaternion.LookRotation(AimDirection),
                null
            );
            projectileGO.SetActive(true);
            EnemyProjectile projectile = projectileGO.GetComponentInChildren<EnemyProjectile>();
            Launch(projectile);
            OnShoot?.Invoke(activator, projectile);
        }

        public void Launch(EnemyProjectile projectile)
        {
            if (target == null)
            {
                Debug.LogError($"Missing target for trap shooter {this}");
                return;
            }
            projectile.AcquireTarget(target);
            projectile.Launch(AimDirection * initialImpulse);
        }

        public EnemyProjectile SpawnProjectile()
        {
            return GetComponentInChildren<EnemyProjectile>();
        }

        public void Destroy()
        {
            if (activator != null)
                activator.InvokeOnTerminate();
            OnActivate -= Activate;
            foreach (TrapTrigger trigger in triggers)
                trigger.gameObject.SetActive(false);
            bee.PlayTeleportDetached();
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }
    }
}
