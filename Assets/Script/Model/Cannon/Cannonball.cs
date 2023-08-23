using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class Cannonball : MonoBehaviour, IProjectile, IExplosive<Cannonball>
    {
        private Rigidbody rb;

        [SerializeField]
        private float countdown;
        public float Countdown => countdown;

        [SerializeField]
        private float damage;
        public float Damage => damage;

        [SerializeField]
        private float blastRadius;
        public float BlastRadius => blastRadius;

        [SerializeField]
        private float blastForce;
        public float BlastForce => blastForce;

        [SerializeField]
        private LayerMask blocking;
        public LayerMask Blocking => blocking;

        [SerializeField]
        private LayerMask affected;
        public LayerMask Affected => affected;
        public Explosion<Cannonball> Explosion => new Explosion<Cannonball>(this, transform.position);
        public IDamaging.Target TargetType => IDamaging.Target.Enemy;
        public event EventHandler OnExplode;
        public event EventHandler<Cannonball> OnDestroy;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            BeginCountdown();
            OnExplode += Explode;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.InLayerMask(blocking))
            {
                // Debug.LogWarning($"Cannonball exploded on contact with {collision.gameObject}");
                OnExplode?.Invoke(collision.gameObject, EventArgs.Empty);
            }
        }

        public void Launch(Vector3 spatialImpulse) =>
            rb.AddForce(spatialImpulse, ForceMode.Impulse);

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

        public void Explode(object sender, EventArgs e)
        {
            OnExplode -= Explode; // only explodes once

            IEnumerable<IDynamic> dynamicEntities = GetAffectedEntityInBlastZone();
            foreach (IDynamic entity in dynamicEntities)
            {
                if (entity is IDamageable damageable)
                {
                    damageable.TakeDamage<Cannonball>(Explosion);
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
            // DebugExtension.DrawWireSphere(transform.position, blastRadius, Color.red, 1f);
            // gameObject.SetTimeOut(.5f, () => Destroy(gameObject)); // destroys self after .5f -> play explosion animation?
            Destroy(gameObject);
        }

        public void Destroy()
        {
            // Debug.LogWarning($"Cannonball blown up by another explosive");
            OnDestroy?.Invoke(this, this);
            OnExplode?.Invoke(this, EventArgs.Empty);
        }

        public void BeginCountdown()
        {
            gameObject.SetTimeOut(
                countdown,
                () =>
                {
                    // Debug.LogWarning("Cannonball expired");
                    OnExplode?.Invoke(this, EventArgs.Empty);
                }
            ); // auto-explode without contact if timer expired
        }
    }
}
