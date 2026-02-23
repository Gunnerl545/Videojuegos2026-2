using UnityEngine;

public class Jugador : MonoBehaviour
{

    void Awake()
    {
        Debug.Log("Awake ejecutado");
    }

    void Start()
    {
        Debug.Log("Start ejecutado");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Colisión ENTER con: " + col.gameObject.name);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("Colisión STAY con: " + col.gameObject.name);
    }

}