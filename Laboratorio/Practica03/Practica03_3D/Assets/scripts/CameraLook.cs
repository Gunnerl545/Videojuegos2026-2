using UnityEngine;
using UnityEngine.InputSystem;
public class CameraLook : MonoBehaviour
{
public float sensitivity = 200f;
float xRotation = 0f;
PlayerInputActions input;
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
Cursor.lockState = CursorLockMode.Locked;
Cursor.visible = false;
}
void Update()
{
Vector2 look = input.Player.Look.ReadValue<Vector2>();
float mouseX = look.x * sensitivity * Time.deltaTime;
float mouseY = look.y * sensitivity * Time.deltaTime;

xRotation -= mouseY;
xRotation = Mathf.Clamp(xRotation, -80f, 80f);
transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
transform.parent.parent.Rotate(Vector3.up * mouseX);
}
}