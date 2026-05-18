using UnityEngine;
using System;
using System.Collections;

// Controla el flujo del juego basado en tiempo y fases
public class TimeManager : MonoBehaviour
{
    [Header("Duración (segundos)")]

    // Duración total del día
    public float dayDuration = 1500f;

    // Duración de la fase de farming
    public float farmingDuration = 1200f;

    // Referencia al mundo
    private GridManager gridManager;

    // Tiempo acumulado del día
    private float currentTime = 0f;

    // Máquina de estados
    public enum Phase
    {
        Farming,
        Defense
    }

    // Estado actual
    public Phase currentPhase;

    // Día actual
    public int currentDay = 1;

    // EVENTOS GLOBALES
    public static event Action OnFarmingStart;
    public static event Action OnDefenseStart;
    public static event Action OnDayEnd;

    // START
    IEnumerator Start()
    {
        // Esperar 1 frame
        yield return null;

        // Obtener referencias
        gridManager = FindObjectOfType<GridManager>();

        // Estado inicial
        currentPhase = Phase.Farming;

        currentTime = 0f;

        // Guardar snapshot del día
        SaveSystem.Instance.SaveGame(currentDay);

        Debug.Log("💾 Estado inicial guardado");

        // Avisar inicio de farming
        OnFarmingStart?.Invoke();
    }

    void Update()
    {
        // Evitar avanzar tiempo durante rollback
        if (SaveSystem.Instance != null &&
            SaveSystem.Instance.isRollingBack)
        {
            return;
        }

        // Avanzar tiempo
        currentTime += Time.deltaTime;

        // Revisar cambios de fase
        CheckPhaseChange();
    }

    void CheckPhaseChange()
    {
        // Farming → Defensa
        if (currentPhase == Phase.Farming &&
            currentTime >= farmingDuration)
        {
            currentPhase = Phase.Defense;

            Debug.Log("🧟 Defensa iniciada");

            // Evento global
            OnDefenseStart?.Invoke();
        }

        // Fin del día
        if (currentTime >= dayDuration)
        {
            EndDay();
        }
    }

    // 🌅 Finalizar día
    void EndDay()
    {
        Debug.Log("🌅 Día terminado");

        // Siguiente día
        currentDay++;

        // Reiniciar tiempo
        currentTime = 0f;

        // Reiniciar fase
        currentPhase = Phase.Farming;

        // Reiniciar spawner
        ZombieSpawner spawner = FindObjectOfType<ZombieSpawner>();

        if (spawner != null)
        {
            spawner.ResetSpawner();
        }

        // Guardar nuevo checkpoint
        SaveSystem.Instance.SaveGame(currentDay);

        Debug.Log("💾 Nuevo día guardado");

        // Eventos
        OnDayEnd?.Invoke();

        OnFarmingStart?.Invoke();
    }

    // 🔁 REINICIAR DÍA TRAS MORIR
    public void ResetDayState(int day)
    {
        Debug.Log("🔁 Reiniciando día");

        // Restaurar mismo día
        currentDay = day;

        // Reiniciar tiempo COMPLETAMENTE
        currentTime = 0f;

        // Volver SIEMPRE a farming
        currentPhase = Phase.Farming;

        // Reactivar farming
        OnFarmingStart?.Invoke();
    }

    // Tiempo restante
    public float GetRemainingTime()
    {
        return dayDuration - currentTime;
    }

    // Obtener tiempo actual
    public float GetCurrentTime()
    {
        return currentTime;
    }

    // Forzar fin del día
    public void ForceEndDay()
    {
        // Evitar bugs durante rollback
        if (SaveSystem.Instance != null &&
            SaveSystem.Instance.isRollingBack)
        {
            return;
        }

        EndDay();
    }
}