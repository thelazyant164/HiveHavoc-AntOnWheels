using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy
{
    [RequireComponent(typeof(Collider))]
    public sealed class TorpedoShooter
        : MonoBehaviour,
            IShootable<Torpedo>,
            IProgressBar,
            IDestructible<TorpedoShooter>
    {
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;

        [Header("Trigger")]
        [SerializeField]
        private List<TorpedoTrigger> triggers;

        [Space]
        [Header("Ammo")]
        [SerializeField]
        private Torpedo projectile;
        public Torpedo Projectile => projectile;

        [SerializeField]
        private float initialImpulse;
        public float InitialImpulse => initialImpulse;

        [SerializeField]
        private Transform spawn;
        public Vector3 ProjectileSpawn => spawn.position;

        [SerializeField]
        private Transform nozzle;
        public Vector3 Nozzle => nozzle.position;
        public Vector3 AimDirection => (nozzle.position - spawn.position).normalized;

        private float remainingCountdown;
        public float Value => remainingCountdown;

        [SerializeField]
        private float countdown;
        public float MaxValue => countdown;

        public float CooldownDuration => 0;
        public bool Ready => true;

        [SerializeField]
        private LayerMask destroyedBy;
        public LayerMask DestroyedBy => destroyedBy;

        private TorpedoTrigger activator;
        private Coroutine countdownInProgress;

        private event EventHandler OnActivate;
        public event EventHandler<Torpedo> OnShoot;
        public event EventHandler<float> OnValueChange;
        public event EventHandler<TorpedoShooter> OnDestroy;

        private void Awake()
        {
            if (triggers.Count == 0)
                return;

            TrapTriggerCountdown countdownProgress = GetComponentInChildren<TrapTriggerCountdown>();
            if (countdownProgress != null)
                countdownProgress.Bind(this);

            OnActivate += Activate;
            foreach (TorpedoTrigger trigger in triggers)
            {
                trigger.OnTrigger += (object sender, EventArgs e) => OnActivate?.Invoke(sender, e);
                OnShoot += (object sender, Torpedo torpedo) =>
                {
                    if ((TorpedoTrigger)sender != trigger)
                        return;
                    trigger.InvokeOnTerminate();
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
            //OnActivate -= Activate; // TODO: solution to handle 2 trigger activating the same shooter; desired behaviour: when 1 trigger, other cannot trigger - current behaviour: both can trigger, unless shooter destroyed
            activator = (TorpedoTrigger)sender;
            StartCoroutine(Countdown(countdown));
        }

        private IEnumerator Countdown(float countdown)
        {
            remainingCountdown = countdown;
            while (remainingCountdown > 0)
            {
                remainingCountdown -= Time.deltaTime;
                OnValueChange?.Invoke(this, remainingCountdown);
                yield return null;
            }
            Shoot();
            gameObject.SetActive(false);
        }

        public void Shoot()
        {
            Torpedo projectile = SpawnProjectile();
            Launch(projectile);
            OnShoot?.Invoke(activator, projectile);
        }

        public void Launch(Torpedo projectile) => projectile.Launch(AimDirection * initialImpulse);

        public Torpedo SpawnProjectile() =>
            GameObject
                .Instantiate(
                    projectile.gameObject,
                    nozzle.position,
                    Quaternion.LookRotation(AimDirection)
                )
                .GetComponent<Torpedo>();

        public void Destroy()
        {
            if (countdownInProgress != null)
                StopCoroutine(countdownInProgress);
            if (activator != null)
                activator.InvokeOnTerminate();
            OnActivate -= Activate;
            foreach (TorpedoTrigger trigger in triggers)
                trigger.gameObject.SetActive(false);
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }
    }
}
