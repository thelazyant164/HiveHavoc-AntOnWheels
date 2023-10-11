using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile
{
    public interface IShootable
    {
        public abstract float InitialImpulse { get; }
        public abstract bool Ready { get; }
        public abstract float CooldownDuration { get; }
        public abstract Vector3 AimDirection { get; }
        public abstract Vector3 ProjectileSpawn { get; }
        public abstract Vector3 Nozzle { get; }

        public abstract void Shoot();
    }

    public interface IShootable<T> : IShootable where T : IProjectile
    {
        public abstract T Projectile { get; }

        public abstract event EventHandler<T> OnShoot;

        public abstract T SpawnProjectile();
        public abstract void Launch(T projectile);
    }
}
