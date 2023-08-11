using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.Unnamed.RacingGame
{
    public enum Role
    {
        Driver,
        Shooter
    }

    public sealed class PlayerManager : Singleton<PlayerManager>
    {
        private PlayerInputManager inputManager;
        public Shooter.Shooter Shooter { get; private set; }
        public Driver.Driver Driver { get; private set; }

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

            inputManager = GetComponent<PlayerInputManager>();
            Shooter = GetComponentInChildren<Shooter.Shooter>();
            Driver = GetComponentInChildren<Driver.Driver>();
        }
    }
}
