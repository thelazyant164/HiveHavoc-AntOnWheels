using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public interface IMovable : IDynamic
    {
        public void ReactTo(Explosion explosion);
    }
}
