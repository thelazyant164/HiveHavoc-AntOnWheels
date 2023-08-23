using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class PollenBurstShooter : PollenShooter<PollenBurst>
    {
        [SerializeField]
        private int ammoCost = 7;
        public override int AmmoCost => ammoCost;
    }
}
