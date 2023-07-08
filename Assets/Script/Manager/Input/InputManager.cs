using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Com.Unnamed.RacingGame.Input
{
    public sealed class InputManager : Singleton<InputManager>
    {
        public event EventHandler<Vector2> OnMouseMove;
        public event EventHandler OnLMBDown;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError(
                    "There's more than one InputManager! " + transform + " - " + Instance
                );
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            if (TryGetMouseMovement(out Vector2 deltaMouseMovement))
            {
                OnMouseMove?.Invoke(this, deltaMouseMovement);
            }
            if (IsMouseButtonDownThisFrame())
            {
                OnLMBDown?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsMouseButtonDownThisFrame() => UnityEngine.Input.GetMouseButtonDown(0);

        public static Vector2 GetMouseScreenPosition() => UnityEngine.Input.mousePosition;

        public static Ray GetRayToMouse(Camera camera, Vector2 mousePos) => camera.ScreenPointToRay(mousePos);
    
        public bool TryGetMouseMovement(out Vector2 deltaMouseMovement)
        {
            deltaMouseMovement.x = UnityEngine.Input.GetAxis("Mouse X");
            deltaMouseMovement.y = UnityEngine.Input.GetAxis("Mouse Y");
            return deltaMouseMovement != Vector2.zero;
        }
    }
}
