using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Gameplay
{
    public sealed class CinemachineManager : Singleton<CinemachineManager>
    {
        [SerializeField]
        private CinemachineVirtualCamera shooterFollowCam;
        [SerializeField]
        private CinemachineVirtualCamera zoomedOutShooterCam;
        [SerializeField]
        private CinemachineVirtualCamera driverFollowCam;
        [SerializeField]
        private CinemachineVirtualCamera zoomedOutDriverCam;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        internal void SwitchCamera(SplitDirection direction)
        {
            if (direction == SplitDirection.Horizontal)
            {
                driverFollowCam.Priority = 1;
                zoomedOutDriverCam.Priority = 0;

                shooterFollowCam.Priority = 0;
                zoomedOutShooterCam.Priority = 1;
            }
            else if (direction == SplitDirection.Vertical)
            {
                driverFollowCam.Priority = 0;
                zoomedOutDriverCam.Priority = 1;

                shooterFollowCam.Priority = 1;
                zoomedOutShooterCam.Priority = 0;                
            }
        }
    }
}
