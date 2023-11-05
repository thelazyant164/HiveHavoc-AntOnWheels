using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile
{
    public struct TargetLockOn
    {
        private Transform projectile;
        private Transform target;
        public Vector3 TargetDirection => (target.position - projectile.position).normalized;
        public Quaternion RotationTowardTarget => Quaternion.LookRotation(TargetDirection);

        public TargetLockOn(ITargetSeeking<IProjectile> projectile, Transform target)
        {
            this.projectile = projectile.Rigidbody.transform;
            this.target = target;
        }
    }

    public interface ITargetSeeking<T> : IProjectile
    {
        public abstract Rigidbody Rigidbody { get; }
        public abstract float Velocity { get; }
        public abstract float PursuitInterval { get; }
        public abstract Coroutine PursuitRoutine { get; }

        public IEnumerator PursueTarget(TargetLockOn targetLock);
        public abstract void AcquireTarget(Transform target);
    }
}
