using UnityEngine;

public class ControlJugador : MonoBehaviour
{

    public float velocidad = 5f;
    public float fuerzaSalto = 8f;
    Animator animator;

    private Rigidbody2D rb;

    private bool enSuelo = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        float h = Input.GetAxis("Horizontal");

        // Movimiento horizontal
        rb.linearVelocity = new Vector2(h * velocidad, rb.linearVelocity.y);

        animator.SetFloat("speed", Mathf.Abs(h));
        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            Debug.Log("Salto ejecutado");
        }
        if (h > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (h < 0)
            transform.localScale = new Vector3(-1, 1, 1);

    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }

    }

    void OnCollisionExit2D(Collision2D col)
    {

        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }

    }

}