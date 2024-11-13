using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float attackRange = 2f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    public float moveSpeed = 3.5f;
    public float turnSpeed = 720f; // Add turnSpeed variable
    public float stoppingDistance = 1.5f; // Add stopping distance to prevent twitching

    private Animator animator;
    private NavMeshAgent agent;
    private EnemyState currentState = EnemyState.Idle;
    private float lastAttackTime;

    // Animation state names
    private const string IdleState = "Idle";
    private const string ChaseState = "Chase";
    private const string AttackState = "Attack";

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.angularSpeed = turnSpeed; // Set the angular speed of the NavMeshAgent
        agent.stoppingDistance = stoppingDistance; // Set the stopping distance

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        currentState = EnemyState.Idle;
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Combat:
                Combat();
                break;
        }
    }

    private void Idle()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(IdleState))
        {
            animator.CrossFade(IdleState, 0.001f);
        }

        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            currentState = EnemyState.Chase;
            agent.isStopped = false;
        }
    }

    private void Chase()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(ChaseState))
        {
            animator.CrossFade(ChaseState, 0.001f);
        }

        if (player != null)
        {
            agent.SetDestination(player.position);
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                currentState = EnemyState.Combat;
                agent.isStopped = true;
            }
            else if (distanceToPlayer > detectionRadius * 1.5f)
            {
                currentState = EnemyState.Idle;
                agent.isStopped = true;
            }
        }
    }

    private void Combat()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(AttackState))
        {
            animator.CrossFade(AttackState, 0.001f);
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = EnemyState.Chase;
            agent.isStopped = false;
        }
    }
}