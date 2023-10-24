using System;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat
{
    public interface ITrackableHostile : IService<ITrackableHostile>
    {
        public Vector3 WorldPosition { get; }
        public event EventHandler OnStartTracking;
        public event EventHandler OnStopTracking;
    }
}
