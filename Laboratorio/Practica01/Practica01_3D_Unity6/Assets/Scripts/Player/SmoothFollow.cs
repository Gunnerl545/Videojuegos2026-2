using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;

    [Header("Distance")]
    public float distance = 5f;
    public float height = 2f;

    [Header("Rotation")]
    public float sensitivity = 120f;
    public float minY = -30f;
    public float maxY = 60f;

    [Header("Smoothing")]
    public float smoothSpeed = 10f;

    float yaw;
    float pitch;

    PlayerInputActions input;

    void Awake()
    {
        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.Player.Enable();

        if (target != null)
            yaw = target.eulerAngles.y;
    }

    void OnDisable()
    {
        input.Player.Disable();
    }

    void LateUpdate()
    {
        if (!target) return;

        Vector2 look = input.Player.Look.ReadValue<Vector2>();

        yaw += look.x * sensitivity * Time.deltaTime;
        pitch -= look.y * sensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minY, maxY);

        target.rotation = Quaternion.Euler(0f, yaw, 0f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 desiredPos =
            target.position +
            Vector3.up * height +
            rotation * new Vector3(0, 0, -distance);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            smoothSpeed * Time.deltaTime);

        transform.LookAt(target.position + Vector3.up * height);
    }
}