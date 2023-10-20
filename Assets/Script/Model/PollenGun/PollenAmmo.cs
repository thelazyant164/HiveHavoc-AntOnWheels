using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    [RequireComponent(typeof(Collider))]
    public sealed class PollenAmmo : MonoBehaviour, IPickUp<PollenAmmo>
    {
        [SerializeField]
        private LayerMask receptible;
        public LayerMask Receptible => receptible;

        [SerializeField]
        private LayerMask destroyedBy;
        public LayerMask DestroyedBy => destroyedBy;

        [SerializeField]
        private float pickUpVFXDuration;
        public float VFXDuration => pickUpVFXDuration;

        [SerializeField]
        private ParticleSystem pickUpVFX;
        public ParticleSystem PickUpVFX => pickUpVFX;

        public event EventHandler<PollenAmmo> OnPickUp;
        public event EventHandler<PollenAmmo> OnDestroy;

        private void Awake()
        {
            OnPickUp += PickUp;
            if (pickUpVFX == null)
            {
                Debug.LogError($"No dissolve particle assigned to {this}");
                return;
            }
            OnDestroy += (object sender, PollenAmmo ammo) => PlayPickUpVFX();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.InLayerMask(receptible))
            {
                // Debug.LogWarning($"Pollen ammo collected on contact with {collision.gameObject}");
                OnPickUp?.Invoke(other.gameObject.GetComponentInChildren<PollenAmmoClip>(), this);
                Destroy();
            }
        }

        public void PickUp(object sender, PollenAmmo ammo)
        {
            OnPickUp -= PickUp;

            if (sender is IDepletableAmmo ammoClip)
            {
                ammoClip.Restock();
            }
        }

        public void Destroy()
        {
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }

        public void PlayPickUpVFX()
        {
            pickUpVFX.transform.SetParent(null, true);
            pickUpVFX.Play();
            gameObject.SetTimeOut(pickUpVFXDuration, () => Destroy(pickUpVFX.gameObject));
        }
    }
}
