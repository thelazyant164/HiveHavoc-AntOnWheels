using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera
{
    public enum CameraPriority
    {
        Hide = 0,
        Show = 1,
    }

    [Serializable]
    public struct CameraAssociation
    {
        public Role role;

        [SerializeField]
        private List<CinemachineVirtualCamera> associated;

        internal void SwitchCamera(CinemachineVirtualCamera mainCamera)
        {
            if (!associated.Contains(mainCamera))
            {
                Debug.LogError($"Trying to set priority {mainCamera} on {role}");
                return;
            }
            foreach (CinemachineVirtualCamera camera in associated)
            {
                camera.Priority = (int)CameraPriority.Hide;
            }
            mainCamera.Priority = (int)CameraPriority.Show;
        }
    }

    public sealed class CameraManager : Singleton<CameraManager>
    {
        [SerializeField]
        private List<CameraAssociation> cameraAssociations;
        internal CameraAssociation this[Role role] =>
            cameraAssociations.Find(cameraAssociation => cameraAssociation.role == role);

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}
