using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class PollenAmmoClip : MonoBehaviour, IDepletableAmmo
    {
        [Header("Current")]
        [SerializeField]
        private int ammo;
        public int Ammo => reloading ? 0 : ammo;

        [SerializeField]
        private int ammoStock;
        public int AmmoStock => ammoStock;

        [SerializeField]
        private bool reloading = false;

        [Space]
        [Header("Clip capacity")]
        [SerializeField]
        private int maxAmmo;
        public int MaxAmmo => maxAmmo;

        [SerializeField]
        private int maxAmmoStock;
        public int MaxAmmoStock => maxAmmoStock;

        [Space]
        [Header("Reload time")]
        [SerializeField]
        private float reloadTime;

        [SerializeField]
        private float restockTime;

        private void Awake()
        {
            ammo = maxAmmo;
            ammoStock = maxAmmoStock;
        }

        public void Consume(int ammo)
        {
            if (Ammo < ammo)
            {
                Debug.LogError($"{this} trying to spend {ammo} from {Ammo} reserve");
                return;
            }
            this.ammo -= ammo;
        }

        public void Reload()
        {
            if (reloading || ammo == maxAmmo)
                return;
            if (ammoStock == 0)
            {
                Debug.LogError($"{this} trying to reload when ammo stock is empty");
                return;
            }
            reloading = true;
            gameObject.SetTimeOut(
                reloadTime,
                () =>
                {
                    ammo = maxAmmo;
                    ammoStock--;
                    reloading = false;
                }
            );
        }

        public void Restock() => gameObject.SetTimeOut(restockTime, () => ammoStock = maxAmmoStock);
    }
}
