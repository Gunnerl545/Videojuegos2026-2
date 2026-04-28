using UnityEngine;

// Controla el movimiento y dirección del jugador
public class PlayerController : MonoBehaviour
{
    // Velocidad de movimiento del jugador
    public float moveSpeed = 5f;

    // Referencia al Rigidbody2D para mover con física
    private Rigidbody2D rb;

    // Vector de movimiento actual (input)
    private Vector2 movement;

    // Última dirección en la que el jugador se movió
    // Se usa para interacción (arar, atacar, etc.)
    private Vector2 lastDirection = Vector2.down; // dirección inicial

    // Propiedad pública de solo lectura
    // Permite a otros scripts saber hacia dónde mira el jugador
    public Vector2 LastDirection => lastDirection;

    // Devuelve el punto frente al jugador a cierta distancia
    // Esto es CLAVE para:
    // - Farming
    // - Ataques
    // - Interacciones
    public Vector2 GetInteractionPoint(float distance)
    {
        return (Vector2)transform.position + lastDirection * distance;
    }

    // Se ejecuta antes de Start
    // Aquí obtenemos referencias necesarias
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Se ejecuta cada frame (input SIEMPRE aquí)
    void Update()
    {
        // Input horizontal (A/D o flechas)
        movement.x = Input.GetAxisRaw("Horizontal");

        // Input vertical (W/S o flechas)
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalizar evita que moverse en diagonal sea más rápido
        movement = movement.normalized;

        // Guardar última dirección válida (si el jugador se mueve)
        if (movement != Vector2.zero)
        {
            lastDirection = movement;
        }
    }

    // Se ejecuta en ciclos de física (mejor para movimiento con Rigidbody)
    void FixedUpdate()
    {
        // Aplicar velocidad al Rigidbody
        rb.linearVelocity = movement * moveSpeed;
    }
}