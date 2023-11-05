using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable, IShootable<EnemyProjectile>
    {
        private NavMeshAgent agent;
        private Transform target;

        [SerializeField]
        private LayerMask ground,
            player;

        [Space]
        [Header("Health")]
        [SerializeField]
        private float maxHealth;
        public float MaxHealth => maxHealth;
        public float Health { get; private set; }

        [Space]
        [Header("Patrol route")]
        [SerializeField]
        private Vector3 walkPoint;
        private bool walkPointSet;

        [SerializeField]
        private float walkPointRange;

        [Space]
        [Header("Attack")]
        [SerializeField]
        private float timeBetweenAttacks;
        public float CooldownDuration => timeBetweenAttacks;

        [Space]
        [Header("Projectile")]
        [SerializeField]
        private Transform projectileSpawnPosition;
        public Vector3 ProjectileSpawn => projectileSpawnPosition.position;

        [SerializeField]
        private GameObject projectile;
        public EnemyProjectile Projectile => projectile.GetComponent<EnemyProjectile>();

        [SerializeField]
        private float initialImpulse;
        public float InitialImpulse => initialImpulse;

        [SerializeField]
        private Transform stinger;

        [SerializeField]
        private Transform stingerTip;
        public Vector3 Nozzle => stingerTip.position;
        public Vector3 AimDirection => (stingerTip.position - stinger.position).normalized;
        public bool Ready { get; private set; } = true;

        [Space]
        [Header("Debug state")]
        [SerializeField]
        private float sightRange;

        [SerializeField]
        private float attackRange;

        [SerializeField]
        private bool playerInSightRange,
            playerInAttackRange;

        public IDamaging.Target Type => IDamaging.Target.Enemy;
        public event EventHandler<EnemyProjectile> OnShoot;
        public event EventHandler<float> OnHealthChange;
        public event EventHandler OnDeath;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            Health = maxHealth;
            OnHealthChange += (object sender, float healthChange) =>
            {
                Health += healthChange;
                if (Health <= 0)
                    OnDeath?.Invoke(this, EventArgs.Empty);
            };
            OnDeath += Die;
        }

        private void Start()
        {
            target = GameManager.Instance.Vehicle?.transform;
        }

        private void Update()
        {
            if (target == null)
                return;

            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, player);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, player);

            if (!playerInSightRange && !playerInAttackRange)
                Patroling();
            else if (playerInSightRange && !playerInAttackRange)
                ChasePlayer();
            else if (playerInAttackRange && playerInSightRange)
                AttackPlayer();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }

        private void Patroling()
        {
            if (!walkPointSet)
                SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }

        private void SearchWalkPoint()
        {
            //Calculate random point in range
            float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
            float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(
                transform.position.x + randomX,
                transform.position.y,
                transform.position.z + randomZ
            );

            if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
                walkPointSet = true;
        }

        private void ChasePlayer()
        {
            agent.SetDestination(target.position);
        }

        private void AttackPlayer()
        {
            agent.SetDestination(transform.position);
            transform.LookAt(target);

            Shoot();
        }

        public EnemyProjectile SpawnProjectile() =>
            GameObject
                .Instantiate(projectile, projectileSpawnPosition.position, Quaternion.identity)
                .GetComponent<EnemyProjectile>();

        public void Launch(EnemyProjectile enemyProjectile)
        {
            enemyProjectile.Launch(AimDirection * initialImpulse);
            enemyProjectile.AcquireTarget(target);
        }

        public void Shoot()
        {
            if (!Ready)
                return;
            Ready = false;
            EnemyProjectile stingerProjectile = SpawnProjectile();
            Launch(stingerProjectile);
            OnShoot?.Invoke(this, stingerProjectile);
            gameObject.SetTimeOut(timeBetweenAttacks, () => Ready = true);
        }

        public void TakeDamage<T>(IDamaging instigator)
        {
            if (instigator.TargetType != Type)
                return; // no friendly fire
            float damage = instigator is Explosion<T> explosion
                ? explosion.GetDamageFrom(transform.position)
                : instigator.Damage;
            OnHealthChange?.Invoke(this, -damage);
        }

        public void Heal(float hp) => OnHealthChange?.Invoke(this, hp);

        public void Die(object sender, EventArgs e)
        {
            OnDeath -= Die; // only die once
            Destroy(gameObject);
            //gameObject.SetTimeOut(1f, () => Destroy(gameObject)); // delay destroy 1s -> play death animation?
        }
    }
}
