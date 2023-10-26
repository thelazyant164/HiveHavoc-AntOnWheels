using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class ReloadService : MonoBehaviour, IServiceProvider<PollenAmmoClip>
    {
        public PollenAmmoClip Service { get; private set; }
        public event EventHandler<PollenAmmoClip> OnAvailable;

        public void Register(PollenAmmoClip ammo)
        {
            Service = ammo;
            OnAvailable?.Invoke(this, ammo);
        }
    }
}
