using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class TorpedoDemoFocus : ScriptedEvent<TorpedoTrigger>
    {
        [SerializeField]
        private TorpedoShooter torpedoShooter;

        [SerializeField]
        private CinemachineVirtualCamera torpedoDemoFocusCamera;
        private CinemachineVirtualCamera mainCamera;

        protected override void Awake()
        {
            base.Awake();
            torpedoShooter.OnShoot += (object sender, Torpedo torpedo) =>
                torpedoDemoFocusCamera.LookAt = torpedo.transform;
        }

        protected override void TriggerCallback()
        {
            mainCamera = cameraManager[Role.Shooter].MainCamera;
            cameraManager[Role.Shooter].SwitchCamera(torpedoDemoFocusCamera);
            PlayerManager.Instance.Shooter.gameObject.SetActive(false);
        }

        protected override void TerminateCallback()
        {
            cameraManager[Role.Shooter].SwitchCamera(mainCamera);
            PlayerManager.Instance.Shooter.gameObject.SetActive(true);
        }
    }
}
