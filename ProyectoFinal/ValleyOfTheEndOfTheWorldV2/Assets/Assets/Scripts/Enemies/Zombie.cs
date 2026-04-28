using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 1f;
    public int health = 3;
    public float stopDistance = 0.5f;
    public int damage = 1;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                PlayerHealth health = other.GetComponentInParent<PlayerHealth>();

                if (health != null)
                {
                    health.TakeDamage(damage);
                }

                lastAttackTime = Time.time;
            }
        }
    }
}