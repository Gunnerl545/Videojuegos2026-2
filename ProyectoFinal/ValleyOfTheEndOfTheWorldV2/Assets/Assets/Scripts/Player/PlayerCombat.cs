using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Distancia de ataque
    public float attackRange = 1f;

    // Daño
    public int damage = 1;

    // Referencia al jugador
    private PlayerController player;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Click izquierdo
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void Attack()
    {
        // Punto frente al jugador
        Vector2 point =
            player.GetInteractionPoint(attackRange);

        // Detectar colliders cercanos
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(point, 0.6f);

        Debug.Log("Hits detectados: " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Golpeó: " + hit.name);

            // =====================================
            // ZOMBIE NORMAL
            // =====================================

            Zombie zombie =
                hit.GetComponent<Zombie>();

            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }

            // =====================================
            // FLANKER
            // =====================================

            FlankerZombie flanker =
                hit.GetComponent<FlankerZombie>();

            if (flanker != null)
            {
                flanker.TakeDamage(damage);
            }
        }
    }

    // Dibujar rango de ataque
    void OnDrawGizmosSelected()
    {
        if (player == null)
        {
            player = GetComponent<PlayerController>();

            if (player == null)
            {
                return;
            }
        }

        Gizmos.color = Color.red;

        Vector2 point =
            player.GetInteractionPoint(attackRange);

        Gizmos.DrawWireSphere(point, 0.6f);
    }
}