using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy;
using System;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class HotspotHighlightService : MonoBehaviour, IServiceProvider<ITrackableHostile>
    {
        public ITrackableHostile Service { get; private set; }
        public event EventHandler<ITrackableHostile> OnAvailable;

        public void Register(ITrackableHostile service)
        {
            Service = service;
            OnAvailable?.Invoke(this, service);
        }
    }
}
