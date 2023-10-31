using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
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
            IDestructible<TrapShooter>,
            ITrackableHostile
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

        [SerializeField]
        private bool terminateWhenShoot = true;

        public Vector3 WorldPosition => transform.position;

        private event EventHandler OnActivate;
        public event EventHandler<EnemyProjectile> OnShoot;
        public event EventHandler<TrapShooter> OnDestroy;
        public event EventHandler OnStartTracking;
        public event EventHandler OnStopTracking;

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
                    if (terminateWhenShoot)
                    {
                        trigger.InvokeOnTerminate();
                    }
                    else
                    {
                        projectile.OnDestroy += (object sender, EnemyProjectile projectile) =>
                            trigger.InvokeOnTerminate();
                    }
                };
            }

            gameObject.SetActive(false);
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
            Register(ServiceManager.Instance.HotspotHighlightService);
            gameObject.SetActive(true);
            bee.PlayShoot();
            //OnActivate -= Activate; // TODO: solution to handle 2 trigger activating the same shooter; desired behaviour: when 1 trigger, other cannot trigger - current behaviour: both can trigger, unless shooter destroyed
            activator = (TrapTrigger)sender;
            OnStartTracking?.Invoke(this, EventArgs.Empty);
        }

        public void Shoot()
        {
            GameObject projectileGO = Instantiate(
                torpedoTrajectory.kinematicProjectile,
                torpedoTrajectory.sample[TrajectoryPoint.Tip],
                Quaternion.LookRotation(AimDirection),
                torpedoTrajectory.kinematicProjectile.transform.parent
            );
            projectileGO.transform.SetParent(null, true);
            projectileGO.SetActive(true);
            EnemyProjectile projectile = projectileGO.GetComponentInChildren<EnemyProjectile>();
            Launch(projectile);
            OnShoot?.Invoke(activator, projectile);
            OnStopTracking?.Invoke(this, EventArgs.Empty);
        }

        public void Launch(EnemyProjectile projectile)
        {
            if (target == null)
            {
                Debug.LogWarning($"Missing target for trap shooter {this}");
                Destroy(projectile.gameObject);
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
            OnStopTracking?.Invoke(this, EventArgs.Empty);
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }

        public void Register(IServiceProvider<ITrackableHostile> provider) =>
            provider.Register(this);
    }
}
