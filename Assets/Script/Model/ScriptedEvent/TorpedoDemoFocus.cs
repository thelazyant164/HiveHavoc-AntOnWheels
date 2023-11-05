using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter;
using UnityEngine.Assertions;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class TorpedoDemoFocus : ScriptedEvent<TrapTrigger>, IServiceConsumer<PollenGun>
    {
        [SerializeField]
        private TrapShooter trapShooter;

        [SerializeField]
        private CinemachineVirtualCamera trapDemoFocusCamera;
        private CinemachineVirtualCamera mainCamera;

        [SerializeField]
        private AudioClip audioAlert;
        private PollenGun gun;

        protected override void Awake()
        {
            base.Awake();
            trapShooter.OnShoot += (object sender, EnemyProjectile projectile) =>
                trapDemoFocusCamera.LookAt = projectile.transform;
        }

        protected override void Start()
        {
            base.Start();
            Register(ServiceManager.Instance.AimService);
        }

        public void Register(IServiceProvider<PollenGun> provider)
        {
            if (provider.Service != null)
            {
                gun = provider.Service;
                return;
            }
            provider.OnAvailable += (object sender, PollenGun gun) => this.gun = gun;
        }

        protected override void TriggerCallback()
        {
            Assert.IsNotNull(gun);

            gun.LookAt(trapShooter.transform.position, cameraManager.ShooterPivotTime);
            gameObject.SetTimeOut(
                cameraManager.ShooterPivotTime,
                () =>
                {
                    mainCamera = cameraManager[Role.Shooter].MainCamera;
                    cameraManager[Role.Shooter].SwitchCamera(trapDemoFocusCamera);

                    UIManager.Instance.VocalAudio.PlayOneShot(audioAlert);

                    EnableControls(Role.Driver, false);
                    EnableControls(Role.Shooter, false);
                    UIManager.Instance.Crosshair.gameObject.SetActive(false); // disable crosshair to takeaway false affordance
                }
            );
        }

        protected override void TerminateCallback()
        {
            cameraManager[Role.Shooter].SwitchCamera(mainCamera);

            EnableControls(Role.Driver, true);
            EnableControls(Role.Shooter, true);
            UIManager.Instance.Crosshair.gameObject.SetActive(true);
        }
    }
}
