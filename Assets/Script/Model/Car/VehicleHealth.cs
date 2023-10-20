using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
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

        [SerializeField]
        private float explosionUpwardForceModifier;
        public float ExplosionUpwardForceModifier => explosionUpwardForceModifier;

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

        public void TakeDamage<T>(IDamaging instigator)
        {
            if (instigator.TargetType != Type)
                return; // no friendly fire
            float damage = instigator is Explosion<T> explosion
                ? explosion.GetDamageFrom(transform.position)
                : instigator.Damage;
            OnHealthChange?.Invoke(this, -damage);
        }

        public void ReactTo<T>(Explosion<T> explosion) =>
            rb.AddExplosionForce(
                explosion.force,
                explosion.epicenter,
                explosion.radius,
                explosionUpwardForceModifier,
                ForceMode.Force
            );

        public void ResetTo(Vector3 position, Quaternion rotation)
        {
            rb.position = position;
            rb.rotation = rotation;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        public void Register(IServiceProvider<RespawnTrigger> provider) { }
    }
}
