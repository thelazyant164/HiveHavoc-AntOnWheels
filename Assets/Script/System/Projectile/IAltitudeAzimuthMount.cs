using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Input;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile
{
    public struct AimDelta
    {
        public Quaternion az;
        public Quaternion alt;

        public AimDelta(Quaternion az, Quaternion alt)
        {
            this.az = az;
            this.alt = alt;
        }
    }

    public interface IAltitudeAzimuthMount
    {
        public abstract Transform Azimuth { get; }
        public abstract Transform Altitude { get; }

        public abstract bool TryAimAz(Quaternion deltaAz);
        public abstract bool TryAimAlt(Quaternion deltaAlt);

        public abstract void OnAim(object sender, AimDelta delta);
    }
}
