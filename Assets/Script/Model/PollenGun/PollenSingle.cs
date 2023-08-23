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

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.InLayerMask(Blocking))
            {
                if (collision.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage<PollenSingle>(this);
                }
                // Debug.LogWarning($"Collide against {collision.gameObject}");
                Destroy();
            }
        }

        public void Destroy()
        {
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }
    }
}
