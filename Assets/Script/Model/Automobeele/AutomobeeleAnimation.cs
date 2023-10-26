using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Animation
{
    [RequireComponent(typeof(Animator))]
    public sealed class AutomobeeleAnimation : MonoBehaviour
    {
        private Animator animator;
        private Shooter.Shooter shooter;

        [SerializeField, Range(-1, 1)]
        private float flyPan; // -1 = all left; 1 = all right

        [SerializeField]
        private float defaultFlySpeed = 1f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            VehicleMovement vehicle = GameManager.Instance.Vehicle;

            shooter = PlayerManager.Instance.Shooter;
            shooter.OnThruster += HandleThruster;

            Driver.Driver driver = PlayerManager.Instance.Driver;
            driver.OnSteer += (object sender, float steerValue) => flyPan = -steerValue;
            driver.OnBrake += (object sender, bool braking) => animator.SetBool("Brake", braking);
        }

        private void OnDestroy()
        {
            shooter.OnThruster -= HandleThruster;
        }

        private void HandleThruster(object sender, float thrusterValue)
        {
            if (thrusterValue == 0)
            {
                animator.SetBool("Fly", false);
            }
            else
            {
                animator.SetBool("Fly", true);
                animator.SetFloat("FlapSpeed", thrusterValue);
                animator.SetFloat(
                    "LeftWing",
                    Mathf.Max(defaultFlySpeed - flyPan, defaultFlySpeed) / 2
                );
                animator.SetFloat(
                    "RightWing",
                    Mathf.Max(defaultFlySpeed + flyPan, defaultFlySpeed) / 2
                );
            }
        }
    }
}
