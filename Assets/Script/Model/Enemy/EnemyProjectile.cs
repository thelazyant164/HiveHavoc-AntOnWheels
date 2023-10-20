using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyProjectile
        : MonoBehaviour,
            ITargetSeeking<IProjectile>,
            IDestructible<EnemyProjectile>
    {
        private Rigidbody rb;
        public Rigidbody Rigidbody => rb;

        [SerializeField]
        private LayerMask interceptedBy;
        public LayerMask InterceptedBy => interceptedBy;
        public LayerMask DestroyedBy => interceptedBy;

        [SerializeField]
        private float damage;
        public float Damage => damage;

        [SerializeField]
        private float velocity;
        public float Velocity => velocity;
        public IDamaging.Target TargetType => IDamaging.Target.Player;

        [SerializeField]
        private float pursuitInterval;
        public float PursuitInterval => pursuitInterval;
        public Coroutine PursuitRoutine { get; private set; }

        [SerializeField]
        private ParticleSystem impactVFX;

        [SerializeField]
        private float impactVFXDuration;

        public event EventHandler<EnemyProjectile> OnDestroy;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            OnDestroy += (object sender, EnemyProjectile projectile) => PlayImpactVFX();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.InLayerMask(interceptedBy))
            {
                if (collision.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage<EnemyProjectile>(this);
                }
                // Debug.LogWarning($"Collide against {collision.gameObject}");
                Destroy();
            }
        }

        public IEnumerator PursueTarget(TargetLockOn targetLock)
        {
            float timeElapsed = 0;
            while (true)
            {
                while (timeElapsed < pursuitInterval)
                {
                    timeElapsed += Time.fixedDeltaTime;
                    rb.MovePosition(
                        rb.transform.position
                            + targetLock.TargetDirection * velocity * Time.fixedDeltaTime
                    );
                    rb.MoveRotation(targetLock.RotationTowardTarget);
                    yield return new WaitForFixedUpdate();
                }
                while (timeElapsed > 0)
                {
                    timeElapsed -= Time.fixedDeltaTime;
                    rb.MovePosition(
                        rb.transform.position
                            + rb.transform.forward * velocity * Time.fixedDeltaTime
                    );
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        public void AcquireTarget(Transform target)
        {
            TargetLockOn targetLock = new TargetLockOn(this, target);
            PursuitRoutine = StartCoroutine(PursueTarget(targetLock));
        }

        public void Launch(Vector3 spatialImpulse)
        {
            rb.MoveRotation(Quaternion.FromToRotation(transform.forward, spatialImpulse));
        }

        public void Destroy()
        {
            if (PursuitRoutine != null)
                StopCoroutine(PursuitRoutine);
            rb.isKinematic = false;
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }

        private void PlayImpactVFX()
        {
            impactVFX.transform.SetParent(null, true);
            impactVFX.transform.rotation = Quaternion.LookRotation(Vector3.up);
            impactVFX.Play();
            gameObject.SetTimeOut(impactVFXDuration, () => Destroy(impactVFX.gameObject));
        }
    }
}
