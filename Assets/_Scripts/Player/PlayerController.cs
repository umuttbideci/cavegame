using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private Transform firePoint; 
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float knockbackForce = 15f;
    [SerializeField] private float knockbackDuration = 0.2f; 
    [SerializeField] private int projectileCount = 5; 
    [SerializeField] private float spreadAngle = 15f; 
    [SerializeField] private float fireRate = 0.5f; 

    
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private bool canShoot = true; 
    private bool isKnockedBack = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isKnockedBack)
        {
            movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = ((Vector2)(mousePosition - transform.position)).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        if (Input.GetMouseButtonDown(0) && canShoot) 
        {
            StartCoroutine(Shoot(direction));
        }
    }

    void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            rb.velocity = movementInput * movementSpeed;
        }
    }

    IEnumerator Shoot(Vector2 direction)
    {
        canShoot = false; 
        isKnockedBack = true; 

        rb.velocity = -direction * knockbackForce;

        for (int i = 0; i < projectileCount; i++)
        {
            float spreadOffset = Random.Range(-spreadAngle, spreadAngle);
            Quaternion rotation = Quaternion.Euler(0, 0, spreadOffset);
            Vector2 spreadDirection = rotation * direction;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();

            if (projRb != null)
            {
                projRb.velocity = spreadDirection * projectileSpeed;
            }
        }

        yield return new WaitForSeconds(knockbackDuration); 
        isKnockedBack = false;
        yield return new WaitForSeconds(fireRate - knockbackDuration); 
        canShoot = true;
    }
}
