using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down; // dirección inicial

    public Vector2 LastDirection => lastDirection;

    public Vector2 GetInteractionPoint(float distance)
    {
        return (Vector2)transform.position + lastDirection * distance;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        // Guardar última dirección válida
        if (movement != Vector2.zero)
        {
            lastDirection = movement;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }
}