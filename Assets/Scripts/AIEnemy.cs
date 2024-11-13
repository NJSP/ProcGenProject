using System;
using UnityEditor;
using UnityEngine;
using Unity.AI;

public class AIEnemy : MonoBehaviour
{
    enum State { Idle, Chase, Attack };
    State currentState = State.Idle;

    private GameObject player;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator animator;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f; // Time between attacks
    private float lastAttackTime;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.avoidancePriority = UnityEngine.Random.Range(20, 30); // Randomize to prevent clumping
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);

                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                animator.SetBool("isChasing", true);
                animator.SetBool("isAttacking", false);
                LookAtPlayer();

                if (distanceToPlayer < attackRange)
                {
                    currentState = State.Attack;
                }
                else if (distanceToPlayer > detectionRange)
                {
                    currentState = State.Idle;
                }
                else
                {
                    ChasePlayer();
                }
                break;

            case State.Attack:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", true);
                LookAtPlayer();

                if (distanceToPlayer > attackRange)
                {
                    navMeshAgent.isStopped = false;
                    currentState = State.Chase;
                }
                else
                {
                    navMeshAgent.isStopped = true;
                    PerformAttack();
                }
                break;
        }
    }

    void ChasePlayer()
    {
        if (navMeshAgent != null && player != null)
        {
            Vector3 worldDirection = (player.transform.position - transform.position).normalized;

            // Convert world direction to local space
            Vector3 localDirection = transform.InverseTransformDirection(worldDirection);

            // Set destination for NavMeshAgent
            navMeshAgent.SetDestination(player.transform.position);

            // Update animator parameters with local directions
            animator.SetFloat("MoveX", localDirection.x);
            animator.SetFloat("MoveZ", localDirection.z);
        }
    }

    void PerformAttack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            // Assume player has a method to take damage
            //player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            lastAttackTime = Time.time;
            Debug.Log("Attacking the player!");
            navMeshAgent.isStopped = true;
            animator.SetTrigger("Attack");
            navMeshAgent.isStopped = false;
        }
    }

    void LookAtPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Adjust the speed as needed
    }
}
