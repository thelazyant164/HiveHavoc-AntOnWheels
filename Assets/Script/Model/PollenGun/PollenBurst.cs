using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class PollenBurst : PollenProjectile, IPrimedExplosive<PollenBurst>
    {
        public ParticleSystem ExplosionVFX => ImpactVFX;
        public AudioSource ExplosionSFX { get; private set; }
        public float ExplosionSFXDuration => ImpactSFXDuration;

        [Space]
        [Header("Explosive")]
        [SerializeField]
        private float countdown;
        public float Countdown => countdown;

        [SerializeField]
        private float blastRadius;
        public float BlastRadius => blastRadius;

        [SerializeField]
        private float blastForce;
        public float BlastForce => blastForce;

        [SerializeField]
        private LayerMask affected;
        public LayerMask Affected => affected;
        public LayerMask Triggering => InterceptedBy;

        public Explosion<PollenBurst> Explosion =>
            new Explosion<PollenBurst>(this, transform.position);

        public event EventHandler OnExplode;
        public event EventHandler<PollenBurst> OnDestroy;

        protected override void Awake()
        {
            ExplosionSFX = GetComponentInChildren<AudioSource>();
            base.Awake();
            BeginCountdown();
            OnExplode += (object sender, EventArgs e) =>
            {
                PlayExplosionVFX();
                PlayExplosionSFX();
            };
            OnExplode += Explode;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.InLayerMask(InterceptedBy))
            {
                //Debug.LogWarning($"Pollen burst exploded on contact with {collision.gameObject}");
                OnExplode?.Invoke(collision.gameObject, EventArgs.Empty);
            }
        }

        public void Explode(object sender, EventArgs e)
        {
            OnExplode -= Explode;

            IEnumerable<IDynamic> dynamicEntities = GetAffectedEntityInBlastZone();
            foreach (IDynamic entity in dynamicEntities)
            {
                if (entity is IDamageable damageable)
                {
                    damageable.TakeDamage<PollenBurst>(Explosion);
                }
                if (entity is IDestructible destructible)
                {
                    destructible.Destroy();
                }
                if (entity is IMovable movable)
                {
                    movable.ReactTo(Explosion);
                }
            }
            //DebugExtension.DrawWireSphere(transform.position, blastRadius, Color.red, 1f);
            Destroy(gameObject);
        }

        public IEnumerable<IDynamic> GetAffectedEntityInBlastZone()
        {
            IEnumerable<Collider> colliderInBlastZone = Physics.OverlapSphere(
                transform.position,
                blastRadius,
                affected
            );
            IEnumerable<IDynamic> dynamicEntities = colliderInBlastZone
                .Select(collider =>
                {
                    if (collider.gameObject.TryFindImmediateComponent(out IDynamic dynamic))
                        return dynamic;
                    return null;
                })
                .Where(entity => entity != null);
            return dynamicEntities;
        }

        public override void Destroy()
        {
            OnDestroy?.Invoke(this, this);
            OnExplode?.Invoke(this, EventArgs.Empty);
        }

        public void BeginCountdown()
        {
            gameObject.SetTimeOut(
                countdown,
                () =>
                {
                    //Debug.LogWarning("Pollen expired");
                    OnExplode?.Invoke(this, EventArgs.Empty);
                }
            );
        }

        public void PlayExplosionVFX()
        {
            ExplosionVFX.transform.SetParent(null, true);
            ExplosionVFX.transform.rotation = Quaternion.LookRotation(Vector3.up);
            ExplosionVFX.Play();
        }

        public void PlayExplosionSFX()
        {
            ExplosionSFX.transform.SetParent(null, true);
            ExplosionSFX.Play();
            GameManager.Instance.gameObject.SetTimeOut(
                ExplosionSFXDuration,
                () => Destroy(ExplosionSFX.gameObject)
            );
        }
    }
}
