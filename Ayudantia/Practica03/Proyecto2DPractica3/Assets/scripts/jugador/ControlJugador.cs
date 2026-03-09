using UnityEngine;

public class ControlJugador : MonoBehaviour
{

    // Movimiento horizontal
    public float velocidadActual = 0f;
    public float velocidadMax = 5f;
    public float aceleracion = 10f;
    public float desaceleracion = 8f;

    // Movimiento vertical
    public float velocidadVertical = 0f;
    public float fuerzaSalto = 10f;

    public float gravedad = -20f;
    public float gravedadCaida = -30f;

    // Sistema de salto avanzado
    public float tiempoCoyote = 0.1f;
    public float tiempoBufferSalto = 0.1f;

    private float coyoteTimer = 0f;
    private float bufferTimer = 0f;

    // DeltaTime manual
    private float tiempoAnterior;

    // Estados para animaciones
    public bool estaCaminando;
    public bool estaSaltando;
    public bool estaCayendo;

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

        // Calculo manual de deltaTime
        float delta = Time.time - tiempoAnterior;
        tiempoAnterior = Time.time;

        float h = Input.GetAxis("Horizontal");

        // ---------------------------
        // ACELERACIÓN HORIZONTAL
        // ---------------------------

        velocidadActual += h * aceleracion * delta;

        // Desaceleración automática
        if (h == 0)
        {
            if (velocidadActual > 0)
                velocidadActual -= desaceleracion * delta;
            else if (velocidadActual < 0)
                velocidadActual += desaceleracion * delta;

            if (Mathf.Abs(velocidadActual) < 0.1f)
                velocidadActual = 0;
        }

        // Limitar velocidad
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMax, velocidadMax);

        // Voltear personaje
        if (h > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (h < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // ---------------------------
        // COYOTE TIME
        // ---------------------------

        if (jugador.enSuelo)
            coyoteTimer = tiempoCoyote;
        else
            coyoteTimer -= delta;

        // ---------------------------
        // JUMP BUFFER
        // ---------------------------

        if (Input.GetAxis("Jump") > 0)
            bufferTimer = tiempoBufferSalto;
        else
            bufferTimer -= delta;

        // ---------------------------
        // SALTO
        // ---------------------------

        if (bufferTimer > 0 && coyoteTimer > 0)
        {
            velocidadVertical = fuerzaSalto;

            jugador.enSuelo = false;

            bufferTimer = 0;
            coyoteTimer = 0;
        }

        // ---------------------------
        // GRAVEDAD
        // ---------------------------

        if (jugador.enSuelo && velocidadVertical <= 0)
        {
            velocidadVertical = 0;
        }
        else
        {
            if (velocidadVertical < 0)
                velocidadVertical += gravedadCaida * delta;
            else
                velocidadVertical += gravedad * delta;
        }

        // ---------------------------
        // MOVIMIENTO FINAL
        // ---------------------------

        transform.position += new Vector3(
            velocidadActual * delta,
            velocidadVertical * delta,
            0
        );

        // ---------------------------
        // ESTADOS DE ANIMACIÓN
        // ---------------------------

        estaCaminando = Mathf.Abs(velocidadActual) > 0.1f;
        estaSaltando = velocidadVertical > 0.1f;
        estaCayendo = velocidadVertical < -0.1f;

        animator.SetFloat("speed", Mathf.Abs(velocidadActual));

    }
}