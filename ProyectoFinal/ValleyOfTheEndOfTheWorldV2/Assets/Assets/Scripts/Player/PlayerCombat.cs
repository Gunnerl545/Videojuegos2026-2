using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 1f;
    public int damage = 1;

    private PlayerController player;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void Attack()
    {
        Vector2 point = player.GetInteractionPoint(attackRange);

        Collider2D[] hits = Physics2D.OverlapCircleAll(point, 0.6f);

        Debug.Log("Hits detectados: " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Golpeó: " + hit.name);

            Zombie zombie = hit.GetComponent<Zombie>();

            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
        }
    }
}