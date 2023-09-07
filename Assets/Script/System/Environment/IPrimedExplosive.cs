using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public interface IPrimedExplosive<T> : IExplosive<T>
    {
        public float Countdown { get; }
        public abstract void BeginCountdown();
    }
}
