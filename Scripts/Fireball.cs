using System;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public float deceleration = 0.2f;
    private Rigidbody2D rb;
    private Collider2D col;

    public int damage = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (rb == null || col == null)
        {
            Debug.LogError("Rigidbody2D or Collider2D not found! Check if both components are added.");
            return;
        }

        rb.gravityScale = 0;
        rb.linearVelocity = transform.right * speed;
        col.enabled = true;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (rb.linearVelocity.magnitude > 0)
        {
            rb.linearVelocity = rb.linearVelocity * (1 - deceleration * Time.deltaTime);
        }

        if (rb.linearVelocity.magnitude <= 0.1f)
        {
            rb.gravityScale = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Use the fireball's velocity to determine the direction of damage
                Vector2 damageDirection = rb.linearVelocity.normalized;
                enemy.TakeDamage(damage, damageDirection); // Pass both damage and direction
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Use the fireball's velocity to determine the direction of damage
                Vector2 damageDirection = rb.linearVelocity.normalized;
                enemy.TakeDamage(damage, damageDirection); // Pass both damage and direction
            }
        }

        Destroy(gameObject);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        if (rb == null || col == null)
        {
            Debug.LogError("Rigidbody2D or Collider2D not found! Ensure both components are added.");
        }
    }

    [Obsolete]
    internal void SetDirection(Vector2 direction)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found. Check initialization.");
            return;
        }

        rb.linearVelocity = direction * speed;
        transform.right = direction;
    }
}
