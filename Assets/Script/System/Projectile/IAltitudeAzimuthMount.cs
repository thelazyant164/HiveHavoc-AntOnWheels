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

    public interface IAltitudeAzimuthMount : IAim
    {
        public Transform Azimuth { get; }
        public Transform Altitude { get; }

        public bool TryAimAz(Quaternion deltaAz);
        public bool TryAimAlt(Quaternion deltaAlt);

        public void OnAim(object sender, AimDelta delta);
    }
}
