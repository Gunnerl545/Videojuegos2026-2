using UnityEngine;

public class FlankerZombie : MonoBehaviour
{
    // =====================================
    // MOVIMIENTO
    // =====================================

    public float moveSpeed = 2.5f;

    public float orbitRadius = 2f;

    // =====================================
    // COMBATE
    // =====================================

    public int maxHealth = 3;

    private int currentHealth;

    public int damage = 1;

    public float attackCooldown = 1f;

    private float nextAttackTime = 0f;

    // =====================================
    // REFERENCIAS
    // =====================================

    private Transform player;

    private PlayerController playerController;

    // Dirección de órbita
    private int orbitDirection;

    void Start()
    {
        // Vida inicial
        currentHealth = maxHealth;

        // Buscar jugador
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;

            playerController =
                playerObj.GetComponent<PlayerController>();
        }

        // Elegir lado aleatorio
        orbitDirection =
            Random.value > 0.5f ? 1 : -1;
    }

    void Update()
    {
        // Validaciones
        if (player == null ||
            playerController == null)
        {
            return;
        }

        // Vector hacia jugador
        Vector2 toPlayer =
            (player.position - transform.position);

        float distance =
            toPlayer.magnitude;

        Vector2 directionToPlayer =
            toPlayer.normalized;

        // Dirección perpendicular
        Vector2 perpendicular =
            new Vector2(
                -directionToPlayer.y,
                directionToPlayer.x);

        perpendicular *= orbitDirection;

        Vector2 moveDirection;

        // Distancia mínima
        float minimumDistance = 1.2f;

        // =====================================
        // IA
        // =====================================

        // Muy lejos → acercarse
        if (distance > orbitRadius)
        {
            moveDirection = directionToPlayer;
        }

        // Muy cerca → alejarse
        else if (distance < minimumDistance)
        {
            moveDirection = -directionToPlayer;
        }

        // Distancia media → orbitar
        else
        {
            moveDirection =
                (directionToPlayer * 0.2f) +
                (perpendicular * 0.8f);
        }

        moveDirection.Normalize();

        // Movimiento
        transform.position +=
            (Vector3)(
                moveDirection *
                moveSpeed *
                Time.deltaTime);
    }

    // =====================================
    // ATAQUE
    // =====================================

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Cooldown
        if (Time.time < nextAttackTime)
        {
            return;
        }

        PlayerHealth health =
            other.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(damage);

            nextAttackTime =
                Time.time + attackCooldown;
        }
    }

    // =====================================
    // RECIBIR DAÑO
    // =====================================

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        Debug.Log("💥 Flanker recibió daño");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // =====================================
    // MORIR
    // =====================================

    void Die()
    {
        Debug.Log("☠️ Flanker muerto");

        ZombieDeathNotifier notifier =
            GetComponent<ZombieDeathNotifier>();

        if (notifier != null &&
            notifier.spawner != null)
        {
            notifier.spawner.OnZombieDied();
        }

        Destroy(gameObject);
    }
}