using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input
{
    public enum Controller
    {
        MouseKeyboard,
        Console
    }

    public sealed class InputManager : Singleton<InputManager>
    {
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

        public static Vector2 GetMouseScreenPosition() => UnityEngine.Input.mousePosition;

        public static Ray GetRayToMouse(UnityEngine.Camera camera, Vector2 mousePos) =>
            camera.ScreenPointToRay(mousePos);
    }
}
