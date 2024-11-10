using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float wanderSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public int attackDamage = 10;
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;
    private Transform player;
    private bool isChasing;
    private Vector3 wanderTarget;
    private float wanderTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isChasing = false;
        wanderTimer = wanderInterval;
        SetNewWanderTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            isChasing = true;
        }
        else if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderInterval)
        {
            SetNewWanderTarget();
            wanderTimer = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, wanderTarget, wanderSpeed * Time.deltaTime);
    }

    void SetNewWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y; // Keep the same height
        wanderTarget = randomDirection;
    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        // Implement attack logic here, e.g., reduce player's health
        Debug.Log("Attacking player with " + attackDamage + " damage.");
    }
}