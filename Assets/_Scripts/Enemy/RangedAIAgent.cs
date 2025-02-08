using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIRangedEnemy : MonoBehaviour
{
    private AIPath path;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float kiteDistance = 3f;

    private float nextFireTime;

    void Start()
    {
        path = GetComponent<AIPath>();
        CircleCollider2D attackCollider = gameObject.AddComponent<CircleCollider2D>();
        attackCollider.radius = attackRange;
        attackCollider.isTrigger = true;
    }

    void Update()
    {
        path.maxSpeed = moveSpeed;
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
            KitePlayer();
        }

        path.destination = target.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void KitePlayer()
    {
        Vector2 directionAway = (transform.position - target.position).normalized;
        Vector2 kitePosition = (Vector2)target.position + directionAway * kiteDistance;
        path.destination = kitePosition;
    }

    private void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null && target != null)
            {
                Vector2 direction = (target.position - firePoint.position).normalized;
                rb.velocity = direction * 10f; // Adjust speed as needed
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}