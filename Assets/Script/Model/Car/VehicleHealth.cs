using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class VehicleHealth : MonoBehaviour, IDamageable, IMovable, IProgressBar
    {
        private Rigidbody rb;

        [SerializeField]
        private float maxHealth;
        public float MaxHealth => maxHealth;
        public float MaxValue => maxHealth;

        public float Health { get; private set; }
        public float Value => Health;

        public IDamaging.Target Type => IDamaging.Target.Player;

        public event EventHandler<float> OnValueChange;
        public event EventHandler<float> OnHealthChange;
        public event EventHandler OnDeath;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            Health = maxHealth;
            OnHealthChange += (object sender, float healthChange) =>
            {
                Health += healthChange;
                OnValueChange?.Invoke(sender, Health);
                if (Health <= 0)
                    OnDeath?.Invoke(this, EventArgs.Empty);
            };
            OnDeath += Die;
        }

        private void Start()
        {
            GameManager.Instance.RegisterVehicle(this);
            UIManager.Instance.HealthBar?.Bind(this);
        }

        public void Die(object sender, EventArgs e)
        {
            OnDeath -= Die;
            gameObject.SetTimeOut(.5f, () => Destroy(gameObject));
        }

        public void Heal(float hp) => OnHealthChange?.Invoke(this, hp);

        public void TakeDamage(IDamaging instigator)
        {
            if (instigator.TargetType != Type)
                return; // no friendly fire
            float damage = instigator is Explosion explosion
                ? explosion.GetDamageFrom(transform.position)
                : instigator.Damage;
            OnHealthChange?.Invoke(this, -damage);
        }

        public void ReactTo(Explosion explosion) =>
            rb.AddExplosionForce(
                explosion.force,
                explosion.epicenter,
                explosion.radius,
                0,
                ForceMode.Force
            );
    }
}
