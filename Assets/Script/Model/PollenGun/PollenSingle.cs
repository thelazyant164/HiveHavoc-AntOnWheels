using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class PollenSingle : PollenProjectile, IDestructible<PollenSingle>
    {
        public event EventHandler<PollenSingle> OnDestroy;

        protected override void Awake()
        {
            base.Awake();
            OnDestroy += (object sender, PollenSingle projectile) => PlayImpactVFX();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.InLayerMask(InterceptedBy))
            {
                if (collision.gameObject.TryFindImmediateComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage<PollenSingle>(this);
                }
                // Debug.LogWarning($"Collide against {collision.gameObject}");
                Destroy();
            }
        }

        public override void Destroy()
        {
            AudioSource.PlayClipAtPoint(ImpactSFX, transform.position);
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }

        private void PlayImpactVFX()
        {
            if (ImpactVFX == null)
            {
                Debug.LogWarning($"No impact VFX assigned to {this}");
                return;
            }
            ImpactVFX.transform.SetParent(null, true);
            ImpactVFX.transform.rotation = Quaternion.LookRotation(Vector3.up);
            ImpactVFX.Play();
            gameObject.SetTimeOut(ExplosionVFXDuration, () => Destroy(ImpactVFX.gameObject));
        }
    }
}
