using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class TorpedoDemoFocus : ScriptedEvent<TrapTrigger>
    {
        [SerializeField]
        private TrapShooter trapShooter;

        [SerializeField]
        private CinemachineVirtualCamera trapDemoFocusCamera;
        private CinemachineVirtualCamera mainCamera;

        [SerializeField]
        private AudioClip audioAlert;

        protected override void Awake()
        {
            base.Awake();
            trapShooter.OnShoot += (object sender, EnemyProjectile projectile) =>
                trapDemoFocusCamera.LookAt = projectile.transform;
        }

        protected override void TriggerCallback()
        {
            mainCamera = cameraManager[Role.Shooter].MainCamera;
            cameraManager[Role.Shooter].SwitchCamera(trapDemoFocusCamera);

            UIManager.Instance.VocalAudio.PlayOneShot(audioAlert);

            PlayerManager.Instance.Shooter.gameObject.SetActive(false); // disable shooter controls
            UIManager.Instance.Crosshair.gameObject.SetActive(false); // disable crosshair to takeaway false affordance
        }

        protected override void TerminateCallback()
        {
            cameraManager[Role.Shooter].SwitchCamera(mainCamera);

            PlayerManager.Instance.Shooter.gameObject.SetActive(true);
            UIManager.Instance.Crosshair.gameObject.SetActive(true);
        }
    }
}
