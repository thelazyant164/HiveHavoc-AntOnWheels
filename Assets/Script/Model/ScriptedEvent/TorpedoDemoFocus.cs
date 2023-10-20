using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class TorpedoDemoFocus : ScriptedEvent<TrapTrigger>
    {
        [SerializeField]
        private TrapShooter trapShooter;

        [SerializeField]
        private CinemachineVirtualCamera trapDemoFocusCamera;
        private CinemachineVirtualCamera mainCamera;

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
            PlayerManager.Instance.Shooter.gameObject.SetActive(false);
        }

        protected override void TerminateCallback()
        {
            cameraManager[Role.Shooter].SwitchCamera(mainCamera);
            PlayerManager.Instance.Shooter.gameObject.SetActive(true);
        }
    }
}
