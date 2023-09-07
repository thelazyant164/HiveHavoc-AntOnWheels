using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public interface IPickUp<T> : ITrigger, IDestructible<T>
    {
        public abstract event EventHandler<T> OnPickUp;

        public abstract void PickUp(object sender, T pickUp);
    }
}
