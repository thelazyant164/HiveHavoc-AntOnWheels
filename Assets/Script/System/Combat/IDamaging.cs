using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat
{
    public interface IDamaging
    {
        public enum Target
        {
            Enemy,
            Player
        }

        public abstract Target TargetType { get; }

        public abstract float Damage { get; }
    }
}
