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
        protected float vehicleRecoilImpulse;

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

        [SerializeField]
        protected AudioSource nozzleAudio;

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

        [Space]
        [Header("VFX")]
        [SerializeField]
        private ParticleSystem muzzleFlash;

        [Space]
        [Header("SFX")]
        [SerializeField]
        private AudioClip shootSFX;

        [SerializeField]
        private AudioClip shootEmptySFX;

        [SerializeField]
        private AudioClip readySFX;

        public event EventHandler<T> OnShoot;

        private void Awake()
        {
            if (muzzleFlash == null)
            {
                Debug.LogError($"Muzzle flash not assigned to {this}");
                return;
            }
            OnShoot += (object sender, T projectile) =>
            {
                muzzleFlash.Play();
                nozzleAudio.PlayOneShot(shootSFX);
            };
        }

        public T SpawnProjectile() =>
            GameObject
                .Instantiate(projectile.gameObject, Nozzle, Quaternion.LookRotation(AimDirection))
                .GetComponent<T>();

        public void Launch(T projectile) => projectile.Launch(AimDirection * InitialImpulse);

        public override void Shoot()
        {
            if (!Ready)
            {
                nozzleAudio.PlayOneShot(shootEmptySFX);
                return;
            }
            Ready = false;
            T projectile = SpawnProjectile();
            Launch(projectile);
            OnShoot?.Invoke(this, projectile);
            GameManager.Instance.Vehicle.ApplyRecoil(vehicleRecoilImpulse * -AimDirection);
            gameObject.SetTimeOut(
                CooldownDuration,
                () =>
                {
                    Ready = true;
                    nozzleAudio.PlayOneShot(readySFX);
                }
            );
        }
    }
}
