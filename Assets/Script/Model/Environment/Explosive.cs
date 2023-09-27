using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Explosive : MonoBehaviour, IExplosive<Explosive>
    {
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        [Header("Explosive config")]
        [SerializeField]
        private float blastRadius;
        public float BlastRadius => blastRadius;

        [SerializeField]
        private float blastForce;
        public float BlastForce => blastForce;

        [SerializeField]
        private LayerMask affected;
        public LayerMask Affected => affected;

        [SerializeField]
        private LayerMask triggering;
        public LayerMask Triggering => triggering;

        public Explosion<Explosive> Explosion => new Explosion<Explosive>(this, transform.position);

        public IDamaging.Target TargetType => IDamaging.Target.Enemy;

        [SerializeField]
        private float damage;
        public float Damage => damage;

        public event EventHandler OnExplode;
        public event EventHandler<Explosive> OnDestroy;

        protected virtual void Awake()
        {
            OnExplode += Explode;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.InLayerMask(triggering))
            {
                // Debug.LogWarning($"Cannonball exploded on contact with {collision.gameObject}");
                OnExplode?.Invoke(collision.gameObject, EventArgs.Empty);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, blastRadius);
        }

        public void Destroy()
        {
            OnDestroy?.Invoke(this, this);
            OnExplode?.Invoke(this, EventArgs.Empty);
        }

        public void Explode(object sender, EventArgs e)
        {
            OnExplode -= Explode; // only explodes once

            IEnumerable<IDynamic> dynamicEntities = GetAffectedEntityInBlastZone();
            foreach (IDynamic entity in dynamicEntities)
            {
                if (entity is IDamageable damageable)
                {
                    damageable.TakeDamage<Explosive>(Explosion);
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
    }
}
