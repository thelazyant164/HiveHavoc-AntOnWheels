using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment
{
    public interface IDynamic
    {
        public GameObject GameObject { get; }
        public Transform Transform { get; }
    }
}
