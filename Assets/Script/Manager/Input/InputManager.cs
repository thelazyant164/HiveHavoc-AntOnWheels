using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Com.Unnamed.RacingGame.Input
{
    public sealed class InputManager : Singleton<InputManager>
    {
        private bool isDragging = false;
        public event EventHandler OnDrag;
        public event EventHandler<Vector2> OnDragMove;
        public event EventHandler OnRelease;

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
            if (!isDragging && IsMouseButtonDownThisFrame())
            {
                isDragging = true;
                OnDrag?.Invoke(this, EventArgs.Empty);
            }
            else if (isDragging && !IsMouseButtonUpThisFrame())
            {
                OnDragMove?.Invoke(this, GetMouseScreenPosition());
            }
            else if (isDragging && IsMouseButtonUpThisFrame())
            {
                isDragging = false;
                OnRelease?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsMouseButtonDownThisFrame() => UnityEngine.Input.GetMouseButtonDown(0);

        public bool IsMouseButtonUpThisFrame() => UnityEngine.Input.GetMouseButtonUp(0);

        private Vector2 GetMouseScreenPosition() => UnityEngine.Input.mousePosition;

        public static Ray GetRayToMouse(Vector2 mousePos) => Camera.main.ScreenPointToRay(mousePos);
    }
}
