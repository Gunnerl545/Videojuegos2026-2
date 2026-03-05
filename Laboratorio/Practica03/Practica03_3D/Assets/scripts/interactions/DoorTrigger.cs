using UnityEngine;
using System;
public class DoorTrigger : MonoBehaviour
{
public event Action OnDoorOpen;
public event Action OnDoorClose;

private void OnTriggerEnter(Collider other)
{
    Debug.Log("Algo entró al trigger");

    if (other.CompareTag("Player"))
    {
        Debug.Log("El jugador entró");
        OnDoorOpen?.Invoke();
    }

if (other.CompareTag("Player"))
{
OnDoorOpen?.Invoke();
}
}
private void OnTriggerExit(Collider other)
{
if (other.CompareTag("Player"))
{
OnDoorClose?.Invoke();
}

}
}