using Com.Unnamed.RacingGame.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Unnamed.RacingGame.Shooter
{
    public sealed class Shooter : MonoBehaviour
    {
        [SerializeField]
        private Cannon cannon;
        internal Cannon Cannon => cannon;

        private InputManager input;
        internal event EventHandler OnAim;
        internal event EventHandler<Vector2> OnMoveAim;
        internal event EventHandler OnShoot;

        private void Start()
        {
            input = InputManager.Instance;
            input.OnDrag += (object sender, EventArgs e) => OnAim?.Invoke(sender, e);
            input.OnDragMove += (object sender, Vector2 mousePos) => OnMoveAim?.Invoke(sender, mousePos);
            input.OnRelease += (object sender, EventArgs e) => OnShoot?.Invoke(sender, e);
        }
    }
}
