using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class AmmoSummary : MonoBehaviour
    {
        [SerializeField]
        private GameObject ammoClip;
        private TextMeshProUGUI text;
        private IDepletableAmmo ammo;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            if (ammoClip.TryGetComponent(out IDepletableAmmo ammo))
            {
                Bind(ammo);
            }
        }

        private void Update()
        {
            text.text = $"{ammo.Ammo}/{ammo.MaxAmmo} ({ammo.AmmoStock}/{ammo.MaxAmmoStock})";
        }

        private void Bind(IDepletableAmmo ammoClip) => ammo = ammoClip;
    }
}
