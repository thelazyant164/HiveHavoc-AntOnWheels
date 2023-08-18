using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public interface IDestructible : IDynamic
    {
        public abstract event EventHandler<IDestructible> OnDestroy;

        public abstract void Destroy();
    }
}
