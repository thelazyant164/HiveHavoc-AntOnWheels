using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public interface ITrigger
    {
        public abstract LayerMask Receptible { get; }
    }
}
