using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class AimService : MonoBehaviour, IServiceProvider<PollenGun>
    {
        public PollenGun Service { get; private set; }
        public event EventHandler<PollenGun> OnAvailable;

        public void Register(PollenGun gun)
        {
            Service = gun;
            OnAvailable?.Invoke(this, gun);
        }
    }
}
