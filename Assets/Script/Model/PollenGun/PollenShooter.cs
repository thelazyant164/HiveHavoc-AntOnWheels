using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    [RequireComponent(typeof(PollenGun))]
    public abstract class PollenShooter : ISwappableShooter
    {
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

        [Space]
        [Header("Cooldown")]
        [SerializeField]
        private float cooldownDuration;
        public float CooldownDuration => cooldownDuration;
        public bool Ready { get; protected set; } = true;
    }

    public abstract class PollenShooter<T> : PollenShooter, IShootable<T> where T : PollenProjectile
    {
        [Space]
        [Header("Ammo")]
        [SerializeField]
        private T projectile;
        public T Projectile => projectile;

        public event EventHandler<T> OnShoot;

        public T SpawnProjectile() =>
            GameObject
                .Instantiate(projectile.gameObject, Nozzle, Quaternion.LookRotation(AimDirection))
                .GetComponent<T>();

        public void Launch(T projectile) => projectile.Launch(AimDirection * InitialImpulse);

        public override void Shoot()
        {
            if (!Ready)
                return;
            Ready = false;
            T projectile = SpawnProjectile();
            Launch(projectile);
            OnShoot?.Invoke(this, projectile);
            gameObject.SetTimeOut(CooldownDuration, () => Ready = true);
        }
    }
}
