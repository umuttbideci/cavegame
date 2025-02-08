using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float patrolRange = 5f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int enemyHealth = 3;
    [SerializeField] private float patrolWaitTime = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private bool canAttack = true;
    private bool isChasing = false;
    private Vector2 patrolPoint;
    private bool isPatrolling = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        StartCoroutine(Patrol()); // Start patrolling initially
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            isChasing = true;
            isPatrolling = false;
            StopCoroutine(Patrol());
            agent.SetDestination(player.position);
        }
        else if (isChasing && distance > detectionRange)
        {
            isChasing = false;
            StartCoroutine(Patrol());
        }

        if (isChasing && distance <= attackRange && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        agent.isStopped = true; // Stop movement during attack
        Debug.Log("Enemy Attacks!");

        // Example: Reduce player's health
        // player.GetComponent<PlayerHealth>().TakeDamage(damage);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        agent.isStopped = false; // Resume movement
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        Debug.Log("Enemy took damage! Remaining HP: " + enemyHealth);

        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died!");
        Destroy(gameObject);
    }

    IEnumerator Patrol()
    {
        isPatrolling = true;

        while (!isChasing)
        {
            patrolPoint = GetRandomPatrolPoint();
            agent.SetDestination(patrolPoint);
            yield return new WaitForSeconds(patrolWaitTime);
        }

        isPatrolling = false;
    }

    Vector2 GetRandomPatrolPoint()
    {
        Vector2 randomDirection = Random.insideUnitCircle * patrolRange;
        Vector2 newPoint = (Vector2)transform.position + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPoint, out hit, patrolRange, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }
}
