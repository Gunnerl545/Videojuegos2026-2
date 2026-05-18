using UnityEngine;

public class FlankerZombie : MonoBehaviour
{
    // Velocidad del enemigo
    public float moveSpeed = 2.5f;

    // Distancia a la que comienza a rodear
    public float orbitRadius = 2f;

    // Daño por ataque
    public int damage = 1;

    // Tiempo entre ataques
    public float attackCooldown = 1f;

    // Próximo momento permitido para atacar
    private float nextAttackTime = 0f;

    // Referencia al jugador
    private Transform player;

    // Referencia al controlador del jugador
    private PlayerController playerController;

    // Dirección de órbita
    // 1 = derecha
    // -1 = izquierda
    private int orbitDirection;

    void Start()
    {
        // Buscar jugador
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;

            playerController =
                playerObj.GetComponent<PlayerController>();
        }

        // Elegir lado aleatorio para rodear
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

        // Vector hacia el jugador
        Vector2 toPlayer =
            (player.position - transform.position);

        // Distancia actual
        float distance =
            toPlayer.magnitude;

        // Dirección normalizada al jugador
        Vector2 directionToPlayer =
            toPlayer.normalized;

        // Dirección perpendicular (para rodear)
        Vector2 perpendicular =
            new Vector2(
                -directionToPlayer.y,
                directionToPlayer.x);

        // Aplicar lado elegido
        perpendicular *= orbitDirection;

        Vector2 moveDirection;

        // =====================================
        // COMPORTAMIENTO
        // =====================================

        // Si está lejos → acercarse
        if (distance > orbitRadius)
        {
            moveDirection = directionToPlayer;
        }
        else
        {
            // Si está cerca → orbitar alrededor
            moveDirection =
                (directionToPlayer * 0.3f) +
                (perpendicular * 0.7f);
        }

        // Normalizar dirección
        moveDirection.Normalize();

        // Movimiento
        transform.position +=
            (Vector3)(
                moveDirection *
                moveSpeed *
                Time.deltaTime);
    }

    // =====================================
    // ATAQUE CON COOLDOWN
    // =====================================
    void OnTriggerStay2D(Collider2D other)
    {
        // Verificar jugador
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Verificar cooldown
        if (Time.time < nextAttackTime)
        {
            return;
        }

        // Obtener vida del jugador
        PlayerHealth health =
            other.GetComponent<PlayerHealth>();

        if (health != null)
        {
            // Hacer daño
            health.TakeDamage(damage);

            // Reiniciar cooldown
            nextAttackTime =
                Time.time + attackCooldown;
        }
    }
}