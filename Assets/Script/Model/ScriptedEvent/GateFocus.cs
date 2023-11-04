using UnityEngine;
using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Gameplay;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter;
using UnityEngine.Assertions;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;

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

        [SerializeField]
        private SplitPreset emphasis = SplitPreset.VerticalShooterOnly;

        [SerializeField]
        private SplitPreset normal = SplitPreset.VerticalEven;

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

                    AdjustScreenSplit(emphasis);
                    mainCamera = cameraManager[Role.Shooter].MainCamera;
                    cameraManager[Role.Shooter].SwitchCamera(gateFocusCamera);

                    if (audioAlert != null)
                        UIManager.Instance.VocalAudio.PlayOneShot(audioAlert);

                    EnableControls(Role.Driver, false);
                    EnableControls(Role.Shooter, false);
                    UIManager.Instance.Crosshair.gameObject.SetActive(false); // disable crosshair to takeaway false affordance

                    gameObject.SetTimeOut(focusDuration, Terminate);
                }
            );
        }

        private void Terminate()
        {
            AdjustScreenSplit(normal);
            cameraManager[Role.Shooter].SwitchCamera(mainCamera);

            EnableControls(Role.Driver, true);
            EnableControls(Role.Shooter, true);
            UIManager.Instance.Crosshair.gameObject.SetActive(true);
        }
    }
}
