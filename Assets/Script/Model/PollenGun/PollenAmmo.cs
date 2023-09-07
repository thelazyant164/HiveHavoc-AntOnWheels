using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    [RequireComponent(typeof(Collider))]
    public sealed class PollenAmmo : MonoBehaviour, IPickUp<PollenAmmo>
    {
        [SerializeField]
        private LayerMask receptible;
        public LayerMask Receptible => receptible;

        public event EventHandler<PollenAmmo> OnPickUp;
        public event EventHandler<PollenAmmo> OnDestroy;

        private void Awake()
        {
            OnPickUp += PickUp;
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
    }
}
