using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy
{
    public sealed class TorpedoShooter : MonoBehaviour, IShootable<Torpedo>
    {
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

        public float CooldownDuration => 0;
        public bool Ready => true;

        public event EventHandler<Torpedo> OnShoot;

        private void Awake()
        {
            if (triggers.Count == 0)
                return;
            foreach (var trigger in triggers)
            {
                trigger.OnTrigger += (object sender, EventArgs e) => Shoot();
            }
            OnShoot += (object sender, Torpedo torpedo) =>
                torpedo.OnDestroy += (object sender, Torpedo torpedo) =>
                    triggers.ForEach(trigger => trigger.InvokeOnTerminate());
        }

        public void Shoot()
        {
            Torpedo projectile = SpawnProjectile();
            Launch(projectile);
            OnShoot?.Invoke(this, projectile);
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
    }
}
