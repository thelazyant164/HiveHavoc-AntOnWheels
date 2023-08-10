
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private LayerMask whatIsGround, whatIsPlayer;

    [SerializeField]
    private float health;

    //Patroling
    [SerializeField]
    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField]
    private float walkPointRange;

    // //Attacking
    // [SerializeField]
    // private float timeBetweenAttacks;
    // private bool alreadyAttacked;
    // [SerializeField]
    // private GameObject projectile;

    //States
    [SerializeField]
    private float sightRange, attackRange;
    [SerializeField]
    private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Car").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

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
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        // if (!alreadyAttacked)
        // {
        //     Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        //     rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        //     rb.AddForce(transform.up * 8f, ForceMode.Impulse);

        //     alreadyAttacked = true;
        //     Invoke(nameof(ResetAttack), timeBetweenAttacks);
        // }
    }

    private void ResetAttack()
    {
        // alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
