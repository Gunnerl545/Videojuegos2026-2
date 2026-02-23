using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
[Header("Movement")]
public float walkSpeed = 5f;
public float runSpeed = 9f;
public float gravity = -9.8f;
public float jumpForce = 7f;
CharacterController controller;
Vector3 velocity;
PlayerInputActions input;
Vector2 moveInput;
void Awake()
{
input = new PlayerInputActions();
}
void OnEnable()

{
input.Player.Enable();
}
void OnDisable()
{
input.Player.Disable();
}
void Start()
{
controller = GetComponent<CharacterController>();
}
void Update()
{
// Movimiento
moveInput = input.Player.Move.ReadValue<Vector2>();
Vector3 move =
transform.right * moveInput.x +
transform.forward * moveInput.y;
bool isRunning = input.Player.Sprint.IsPressed();
float speed = isRunning ? runSpeed : walkSpeed;
controller.Move(move * speed * Time.deltaTime);
// Gravedad
if (controller.isGrounded && velocity.y < 0)
velocity.y = -2f;
// Salto
if (input.Player.Jump.triggered && controller.isGrounded)
velocity.y = jumpForce;
velocity.y += gravity * Time.deltaTime;
controller.Move(velocity * Time.deltaTime);
}
}