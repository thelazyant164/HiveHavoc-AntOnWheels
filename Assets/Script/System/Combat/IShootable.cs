using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat
{
    public interface IShootable<T> where T : IProjectile
    {
        public abstract float InitialImpulse { get; }
        public abstract T Projectile { get; }
        public abstract bool Ready { get; }
        public abstract float CooldownDuration { get; }
        public abstract Vector3 AimDirection { get; }
        public abstract Vector3 ProjectileSpawnPosition { get; }
        public abstract event EventHandler<T> OnShoot;

        public abstract T SpawnProjectile();
        public abstract void Launch(T projectile);
    }
}
