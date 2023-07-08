using Com.Unnamed.RacingGame.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace Com.Unnamed.RacingGame.Shooter
{
    public sealed class Shooter : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField]
        private Camera shooterCamera;
        [SerializeField]
        private Transform cameraAz;
        [SerializeField]
        private Transform cameraAlt;

        private InputManager input;
        internal event EventHandler<Ray> OnAim;
        internal event EventHandler OnShoot;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            input = InputManager.Instance;
            input.OnMouseMove += (object sender, Vector2 deltaMouseMovement) => 
            {
                cameraAz.localRotation *= Quaternion.Euler(0, deltaMouseMovement.x, 0);
                cameraAlt.localRotation *= Quaternion.Euler(-deltaMouseMovement.y, 0, 0);
                OnAim?.Invoke(sender, InputManager.GetRayToMouse(shooterCamera, InputManager.GetMouseScreenPosition()));
            };
            input.OnLMBDown += (object sender, EventArgs e) => OnShoot?.Invoke(sender, e);
        }
    }
}
