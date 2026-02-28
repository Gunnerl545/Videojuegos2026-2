using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    // Movimiento horizontal
    public float velocidadActual = 0f;
    public float velocidadMax = 5f;
    public float aceleracion = 10f;

    // Movimiento vertical
    public float velocidadVertical = 0f;
    public float gravedad = -20f;

    public float tiempoMaxSalto = 0.2f;
    private float tiempoSaltoActual = 0f;

    private float tiempoAnterior;

    private Jugador jugador;
    private Animator animator;

    void Awake()
    {
        jugador = GetComponent<Jugador>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        tiempoAnterior = Time.time;
    }

    void Update()
    {
        // Calcular delta manual
        float delta = Time.time - tiempoAnterior;
        tiempoAnterior = Time.time;

        // ---------------------
        // MOVIMIENTO HORIZONTAL
        // ---------------------

        float h = Input.GetAxis("Horizontal");

        velocidadActual += h * aceleracion * delta;
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMax, velocidadMax);

        transform.position += new Vector3(velocidadActual * delta, 0, 0);

        animator.SetFloat("speed", Mathf.Abs(velocidadActual));

        if (h > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (h < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // ---------------------
        // SALTO
        // ---------------------

        if (Input.GetAxis("Jump") > 0 && jugador.enSuelo)
        {
            velocidadVertical = 10f;
            jugador.enSuelo = false;
            tiempoSaltoActual = 0f;
        }

        if (!jugador.enSuelo && Input.GetAxis("Jump") > 0)
        {
            if (tiempoSaltoActual < tiempoMaxSalto)
            {
                velocidadVertical += 20f * delta;
                tiempoSaltoActual += delta;
            }
        }

        if (Input.GetAxis("Jump") == 0)
        {
            tiempoSaltoActual = tiempoMaxSalto;
        }

        if (jugador.enSuelo)
        {
            velocidadVertical = 0;
        }
        else
        {
            velocidadVertical += gravedad * delta;
        }

        transform.position += new Vector3(0, velocidadVertical * delta, 0);
    }
}