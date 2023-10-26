using UnityEngine;
using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Gameplay;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter;
using UnityEngine.Assertions;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class GateFocus : ScriptedEvent<Checkpoint>, IServiceConsumer<PollenGun>
    {
        [SerializeField]
        private float focusDuration = 3f;

        [SerializeField]
        private GateTimer gate;

        [SerializeField]
        private CinemachineVirtualCamera gateFocusCamera;
        private CinemachineVirtualCamera mainCamera;

        [SerializeField]
        private AudioClip audioAlert;

        private bool triggered = false;
        private PollenGun gun;

        protected override void Awake()
        {
            base.Awake();
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

            if (triggered)
            {
                return;
            }
            triggered = true;

            gun.LookAt(gate.transform.position, cameraManager.ShooterPivotTime);
            gameObject.SetTimeOut(
                cameraManager.ShooterPivotTime,
                () =>
                {
                    gate.StartTimer();

                    mainCamera = cameraManager[Role.Shooter].MainCamera;
                    cameraManager[Role.Shooter].SwitchCamera(gateFocusCamera);

                    if (audioAlert != null)
                        UIManager.Instance.VocalAudio.PlayOneShot(audioAlert);

                    PlayerManager.Instance.Shooter.gameObject.SetActive(false); // disable shooter controls
                    UIManager.Instance.Crosshair.gameObject.SetActive(false); // disable crosshair to takeaway false affordance

                    gameObject.SetTimeOut(focusDuration, Terminate);
                }
            );
        }

        private void Terminate()
        {
            cameraManager[Role.Shooter].SwitchCamera(mainCamera);

            PlayerManager.Instance.Shooter.gameObject.SetActive(true);
            UIManager.Instance.Crosshair.gameObject.SetActive(true);
        }
    }
}
