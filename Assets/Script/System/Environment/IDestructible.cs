using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public interface IDestructible : IDynamic 
    {
        public abstract void Destroy();
    }

    public interface IDestructible<T> : IDestructible
    {
        public abstract event EventHandler<T> OnDestroy;
    }
}
