using UnityEngine;
using System;

// Controla el flujo del juego basado en tiempo y fases
public class TimeManager : MonoBehaviour
{
    [Header("Duración (segundos)")]

    // Duración total del día (25 min)
    public float dayDuration = 1500f;

    // Duración de la fase de farming (20 min)
    public float farmingDuration = 1200f;

    // Referencia al mundo (no es estrictamente necesaria aquí, pero útil)
    private GridManager gridManager;

    // Tiempo acumulado desde el inicio del día
    private float currentTime = 0f;

    // Fases del juego (máquina de estados)
    public enum Phase
    {
        Farming,
        Defense
    }

    // Fase actual del juego
    public Phase currentPhase;

    // Contador de días
    public int currentDay = 1;

    // EVENTOS GLOBALES (clave para desacoplar sistemas)
    public static event Action OnFarmingStart;
    public static event Action OnDefenseStart;
    public static event Action OnDayEnd;

    void Start()
    {
        // Inicializar en fase de farming
        currentPhase = Phase.Farming;

        // Notificar a otros sistemas (ej: PlayerFarming)
        OnFarmingStart?.Invoke();

        // Obtener referencia al GridManager
        gridManager = FindObjectOfType<GridManager>();

        currentPhase = Phase.Farming;

        // Guardar estado inicial del día (snapshot)
        SaveSystem.Instance.SaveGame(currentDay);

        // Notificar inicio de farming (otra vez, redundante)
        OnFarmingStart?.Invoke();
    }

    void Update()
    {
        // Avanzar el tiempo del día
        currentTime += Time.deltaTime;

        // Verificar si hay cambio de fase
        CheckPhaseChange();
    }

    void CheckPhaseChange()
    {
        // Si estamos en farming y se acabó el tiempo → pasar a defensa
        if (currentPhase == Phase.Farming && currentTime >= farmingDuration)
        {
            currentPhase = Phase.Defense;

            Debug.Log("🧟 Defensa iniciada");

            // Notificar a otros sistemas (ej: ZombieSpawner)
            OnDefenseStart?.Invoke();
        }
    }

    // Finalizar el día (cuando se eliminan todos los zombies)
    void EndDay()
    {
        Debug.Log("🌅 Día terminado");

        // Avanzar al siguiente día
        currentDay++;

        // Reiniciar tiempo
        currentTime = 0f;

        // Volver a fase de farming
        currentPhase = Phase.Farming;

        // Guardar nuevo estado (nuevo checkpoint)
        SaveSystem.Instance.SaveGame(currentDay);

        // Notificar fin de día
        OnDayEnd?.Invoke();

        // Notificar inicio de nueva fase de farming
        OnFarmingStart?.Invoke();
    }

    // Devuelve el tiempo restante del día
    public float GetRemainingTime()
    {
        return dayDuration - currentTime;
    }

    // Permite forzar el fin del día desde otros scripts
    // (ej: cuando mueren todos los zombies)
    public void ForceEndDay()
    {
        EndDay();
    }
}