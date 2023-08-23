using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using System;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public struct Explosion<T> : IDamaging
    {
        public IDamaging.Target TargetType { get; private set; }
        public float Damage { get; private set; }
        public Vector3 epicenter;
        public float force;
        public float radius;

        public Explosion(IExplosive<T> source, Vector3 epicenter)
        {
            this.epicenter = epicenter;
            Damage = source.Damage;
            force = source.BlastForce;
            radius = source.BlastRadius;
            TargetType = source.TargetType;
        }

        public float GetDamageFrom(Vector3 position)
        {
            float power = 1 - Vector3.Distance(position, epicenter) / radius;
            return Mathf.Clamp(power, 0, 1) * Damage;
        }
    }

    public interface IExplosive<T> : IDamaging, IDestructible<T>
    {
        public float Countdown { get; }
        public float BlastRadius { get; }
        public float BlastForce { get; }
        public LayerMask Affected { get; }
        public Explosion<T> Explosion { get; }
        public abstract event EventHandler OnExplode;

        public abstract IEnumerable<IDynamic> GetAffectedEntityInBlastZone();
        public abstract void Explode(object sender, EventArgs e);
        public abstract void BeginCountdown();
    }
}
