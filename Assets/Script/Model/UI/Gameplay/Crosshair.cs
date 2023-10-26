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

    public sealed class Crosshair
        : MonoBehaviour,
            IServiceConsumer<PollenGun>,
            IServiceConsumer<PollenAmmoClip>
    {
        [SerializeField]
        private Image normal;

        [SerializeField]
        private Image highlighted;

        [SerializeField]
        private Image reloading;

        private bool aimingInterest;
        private bool reloadInProgress;

        private void Awake()
        {
            StartCoroutine(Spin());
        }

        private void Start()
        {
            Register(ServiceManager.Instance.AimService);
            Register(ServiceManager.Instance.ReloadService);
        }

        private void Update()
        {
            reloading.gameObject.SetActive(reloadInProgress);
            normal.gameObject.SetActive(!reloadInProgress && !aimingInterest);
            highlighted.gameObject.SetActive(!reloadInProgress && aimingInterest);
        }

        public void Register(IServiceProvider<PollenGun> serviceProvider)
        {
            if (serviceProvider.Service != null)
            {
                serviceProvider.Service.OnAimTargetChange += (object sender, AimTarget target) =>
                    aimingInterest = target == AimTarget.ObjectOfInterest;
                return;
            }
            serviceProvider.OnAvailable += (object sender, PollenGun aimableComponent) =>
            {
                aimableComponent.OnAimTargetChange += (object sender, AimTarget target) =>
                    aimingInterest = target == AimTarget.ObjectOfInterest;
            };
        }

        public void Register(IServiceProvider<PollenAmmoClip> serviceProvider)
        {
            if (serviceProvider.Service != null)
            {
                serviceProvider.Service.OnReload += (object sender, bool reloadStatus) =>
                    reloadInProgress = reloadStatus;
                return;
            }
            serviceProvider.OnAvailable += (object sender, PollenAmmoClip ammoClip) =>
            {
                ammoClip.OnReload += (object sender, bool reloadStatus) =>
                    reloadInProgress = reloadStatus;
                ;
            };
        }

        private IEnumerator Spin()
        {
            while (true)
            {
                if (reloadInProgress)
                {
                    reloading.transform.Rotate(Vector3.forward, -1f);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
