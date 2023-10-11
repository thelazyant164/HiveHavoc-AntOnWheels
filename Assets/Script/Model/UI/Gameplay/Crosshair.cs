using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public enum AimTarget
    {
        None,
        ObjectOfInterest,
    }

    public sealed class Crosshair : MonoBehaviour, IServiceConsumer<PollenGun>
    {
        [SerializeField]
        private Image normal;

        [SerializeField]
        private Image highlighted;

        private void Start()
        {
            Register(GameManager.Instance);
        }

        public void Register(IServiceProvider<PollenGun> serviceProvider)
        {
            serviceProvider.OnAvailable += (object sender, PollenGun aimableComponent) =>
            {
                aimableComponent.OnAimTargetChange += (object sender, AimTarget target) =>
                {
                    normal.gameObject.SetActive(target == AimTarget.None);
                    highlighted.gameObject.SetActive(target == AimTarget.ObjectOfInterest);
                };
            };
        }
    }
}
