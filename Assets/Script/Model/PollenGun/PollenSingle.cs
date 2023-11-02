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
        private AudioSource impactSFX;
        public event EventHandler<PollenSingle> OnDestroy;

        protected override void Awake()
        {
            impactSFX = GetComponentInChildren<AudioSource>();
            base.Awake();
            OnDestroy += (object sender, PollenSingle projectile) =>
            {
                PlayImpactVFX();
                PlayImpactSFX();
            };
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
        }

        private void PlayImpactSFX()
        {
            impactSFX.transform.SetParent(null, true);
            impactSFX.Play();
            GameManager.Instance.gameObject.SetTimeOut(
                ImpactSFXDuration,
                () => Destroy(impactSFX.gameObject)
            );
        }
    }
}
