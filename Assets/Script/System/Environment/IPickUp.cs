using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public interface IPickUp<T> : ITrigger, IDestructible<T>
    {
        public float VFXDuration { get; }
        public ParticleSystem PickUpVFX { get; }
        public event EventHandler<T> OnPickUp;

        public void PickUp(object sender, T pickUp);
        public void PlayPickUpVFX();
    }
}
