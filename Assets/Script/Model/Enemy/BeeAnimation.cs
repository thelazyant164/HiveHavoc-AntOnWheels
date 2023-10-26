using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy
{
    [RequireComponent(typeof(Animator))]
    public sealed class BeeAnimation : MonoBehaviour
    {
        private Animator animator;
        private AnimatedProjectile projectile;

        [Space]
        [Header("Teleport")]
        [SerializeField]
        private ParticleSystem teleportVFX;

        [SerializeField]
        private AudioClip teleportSFX;

        [SerializeField]
        private float teleportDuration;
        private Vector3 originalScale;

        [Space]
        [Header("Ambience")]
        [SerializeField]
        private AudioClip buzz;

        internal event EventHandler<Trajectory> OnEstablish;

        private void Awake()
        {
            originalScale = transform.parent.localScale;
            animator = GetComponent<Animator>();

            projectile = GetComponentInChildren<AnimatedProjectile>();
            if (projectile != null)
            {
                projectile.OnEstablish += (object sender, Trajectory trajectory) =>
                    OnEstablish?.Invoke(sender, trajectory);
            }
        }

        internal void RecordTrajectory(TrajectoryPoint point)
        {
            if (projectile == null)
            {
                Debug.LogError(
                    $"Attempted to record projectile trajectory on animator without animated projectile"
                );
                return;
            }
            projectile.RecordTrajectory(point);
        }

        internal void PlayShoot()
        {
            if (projectile != null)
            {
                projectile.Enable();
            }
            transform.parent.localScale = originalScale;
            animator.SetTrigger("Shoot");

            AudioSource.PlayClipAtPoint(buzz, GameManager.Instance.Vehicle.transform.position);
        }

        // TODO: (LIMITATION) due to hotfix of "modifying local scale" to simulate disappear after shoot & only reappear when shoot again, when player die & respawn cannot shoot ahead
        internal void PlayTeleport()
        {
            AudioSource.PlayClipAtPoint(teleportSFX, transform.position);
            teleportVFX.Play();
            // TODO: (LIMITATION) decouple hierarchy from anim play when teleport away
            transform.parent.localScale = Vector3.zero;
        }

        internal void PlayTeleportDetached()
        {
            AudioSource.PlayClipAtPoint(teleportSFX, transform.position);
            teleportVFX.transform.SetParent(null, true);
            teleportVFX.Play();
            gameObject.SetTimeOut(teleportDuration, () => Destroy(teleportVFX.gameObject));
        }
    }
}
