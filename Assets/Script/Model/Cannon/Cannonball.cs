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
    public sealed class Cannonball : Explosive, IProjectile, IPrimedExplosive<Cannonball>
    {
        [SerializeField]
        private float countdown;
        public float Countdown => countdown;

        [SerializeField]
        private LayerMask interceptedBy;
        public LayerMask InterceptedBy => interceptedBy;
        public new Explosion<Cannonball> Explosion =>
            new Explosion<Cannonball>(this, transform.position);
        public new IDamaging.Target TargetType => IDamaging.Target.Enemy;

        public new event EventHandler<Cannonball> OnDestroy;

        protected override void Awake()
        {
            base.Awake();
            BeginCountdown();
            OnDestroy?.Invoke(this, this); // silence warning
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.InLayerMask(interceptedBy))
            {
                // Debug.LogWarning($"Cannonball exploded on contact with {collision.gameObject}");
                Explode();
            }
        }

        public void Launch(Vector3 spatialImpulse) =>
            Rb.AddForce(spatialImpulse, ForceMode.Impulse);

        public override void Explode(object sender, EventArgs e)
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

        public void BeginCountdown()
        {
            gameObject.SetTimeOut(
                countdown,
                () =>
                {
                    // Debug.LogWarning("Cannonball expired");
                    Explode();
                }
            ); // auto-explode without contact if timer expired
        }
    }
}
