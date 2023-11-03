using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class AmmoIndicator : MonoBehaviour
    {
        [SerializeField]
        private Transform ammoVolume;

        [SerializeField]
        private Transform demoVolume;

        [SerializeField]
        private PollenAmmoClip ammo;

        private float SingleWidth => demoVolume.localScale.y;
        private float BottomHeight => demoVolume.localPosition.y - (SingleWidth / 2);

        private void Awake()
        {
            Assert.IsNotNull(ammo);
        }

        private void Update()
        {
            UpdateAmmoLevel();
        }

        private void UpdateAmmoLevel()
        {
            if (ammo.Ammo == 0)
            {
                ammoVolume.gameObject.SetActive(false);
                return;
            }

            ammoVolume.gameObject.SetActive(true);

            Vector3 scale = ammoVolume.localScale;
            scale.y = ammo.Ammo * SingleWidth;
            ammoVolume.localScale = scale;

            Vector3 position = ammoVolume.localPosition;
            position.y = (ammo.Ammo - 1) / 2 * SingleWidth + BottomHeight;
            ammoVolume.localPosition = position;
        }
    }
}
