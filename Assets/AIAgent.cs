using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIAgent : MonoBehaviour
{
    private AIPath path;
    private bool isChasing;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private float attackRange;
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

        if (isChasing)
        {
            path.destination = target.position;
        }
    }       

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = true;
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
