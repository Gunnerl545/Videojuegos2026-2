using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class ClickToMove : MonoBehaviour
{
public NavMeshAgent agent;
void Update()
{
if (Mouse.current.leftButton.wasPressedThisFrame)
{
Ray ray =

Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
if (Physics.Raycast(ray, out RaycastHit hit))
{
agent.SetDestination(hit.point);
}
}
}
}