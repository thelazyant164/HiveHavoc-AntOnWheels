using System;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile
{
    public interface IAim
    {
        public float MaxAimDistance { get; }
        public LayerMask AimInterest { get; }

        public event EventHandler<AimTarget> OnAimTargetChange;

        public bool TryGetAimTarget(out AimTarget result);
    }
}
