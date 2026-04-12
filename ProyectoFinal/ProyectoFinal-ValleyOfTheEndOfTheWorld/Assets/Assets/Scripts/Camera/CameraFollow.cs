using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        Vector3 newPosition = target.position;
        newPosition.z = -8;

        transform.position = newPosition;
    }
}