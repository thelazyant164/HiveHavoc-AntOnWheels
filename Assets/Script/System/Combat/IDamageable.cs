using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat
{
    public interface IDamageable : IDynamic
    {
        public abstract float MaxHealth { get; }
        public abstract float Health { get; }
        public abstract IDamaging.Target Type { get; }
        public abstract event EventHandler<float> OnHealthChange;
        public abstract event EventHandler OnDeath;

        public abstract void TakeDamage(IDamaging instigator);
        public abstract void Heal(float hp);
        public abstract void Die(object sender, EventArgs e);
    }
}
