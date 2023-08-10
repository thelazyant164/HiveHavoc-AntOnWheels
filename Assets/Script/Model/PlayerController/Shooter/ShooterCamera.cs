using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Unnamed.RacingGame.Shooter
{
    [RequireComponent(typeof(Camera))]
    public sealed class ShooterCamera : MonoBehaviour
    {
        private Shooter shooter;

        private void Start()
        {
            shooter = PlayerManager.Instance.Shooter;
            shooter.SetupCamera(transform.parent.parent, transform.parent, GetComponent<Camera>());
        }

        // Update is called once per frame
        void Update() { }
    }
}
