using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter
{
    public sealed class PollenSingleShooter : PollenShooter<PollenSingle>
    {
        [SerializeField]
        private int ammoCost = 1;
        public override int AmmoCost => ammoCost;
    }
}
