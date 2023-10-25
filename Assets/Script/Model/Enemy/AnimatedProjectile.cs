using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrajectoryPoint
{
    Origin,
    Tip
}

public struct Trajectory
{
    public Dictionary<TrajectoryPoint, Vector3> sample;
    public GameObject kinematicProjectile;

    public Trajectory(Vector3 origin, Vector3 tip, GameObject projectile)
    {
        sample = new() { [TrajectoryPoint.Origin] = origin, [TrajectoryPoint.Tip] = tip };
        kinematicProjectile = projectile;
    }
}

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy
{
    public sealed class AnimatedProjectile : MonoBehaviour
    {
        private Vector3 origin;
        private Vector3 tip;

        internal event EventHandler<Trajectory> OnEstablish;

        [SerializeField]
        private AudioClip shootSFX;

        internal void Enable() => gameObject.SetActive(true);

        internal void Disable() => gameObject.SetActive(false);

        internal void RecordTrajectory(TrajectoryPoint point)
        {
            if (point == TrajectoryPoint.Origin)
            {
                origin = transform.position;
            }
            else
            {
                tip = transform.position;
                Disable();
                OnEstablish?.Invoke(this, new Trajectory(origin, tip, gameObject));
                AudioSource.PlayClipAtPoint(shootSFX, tip);
            }
        }
    }
}
