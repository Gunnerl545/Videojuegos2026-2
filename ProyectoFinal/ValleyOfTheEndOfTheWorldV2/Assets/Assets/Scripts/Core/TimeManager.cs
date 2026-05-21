using UnityEngine;
using System;
using System.Collections;

// Controla el flujo del juego basado en tiempo y fases
public class TimeManager : MonoBehaviour
{
    [Header("Duración (segundos)")]

    // Duración total del día
    public float dayDuration = 1500f;

    // Tiempo antes de iniciar defensa
    public float farmingDuration = 1200f;

    // Referencia al grid
    private GridManager gridManager;

    // Tiempo acumulado
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

    // =====================================
    // START
    // =====================================

    IEnumerator Start()
    {
        // Esperar 1 frame
        yield return null;

        // Referencias
        gridManager =
            FindObjectOfType<GridManager>();

        // Estado inicial
        currentPhase = Phase.Farming;

        currentTime = 0f;

        // Guardar checkpoint inicial
        SaveSystem.Instance.SaveGame(currentDay);

        Debug.Log("💾 Estado inicial guardado");

        // Iniciar farming
        OnFarmingStart?.Invoke();
    }

    // =====================================
    // UPDATE
    // =====================================

    void Update()
    {
        // Evitar avanzar tiempo durante rollback
        if (SaveSystem.Instance != null &&
            SaveSystem.Instance.isRollingBack)
        {
            return;
        }

        // =====================================
        // IMPORTANTE:
        // SOLO avanza el tiempo en FARMING
        // =====================================

        if (currentPhase == Phase.Farming)
        {
            currentTime += Time.deltaTime;
        }

        // Revisar fases
        CheckPhaseChange();
    }

    // =====================================
    // CAMBIO DE FASE
    // =====================================

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

        // IMPORTANTE:
        // YA NO existe:
        //
        // if(currentTime >= dayDuration)
        // {
        //     EndDay();
        // }
        //
        // El día termina SOLO cuando
        // todos los zombies mueren.
    }

    // =====================================
    // 🌅 FINALIZAR DÍA
    // =====================================

    void EndDay()
    {
        Debug.Log("🌅 Día terminado");

        // Siguiente día
        currentDay++;

        // Reiniciar tiempo
        currentTime = 0f;

        // Volver a farming
        currentPhase = Phase.Farming;

        // Reiniciar spawner
        ZombieSpawner spawner =
            FindObjectOfType<ZombieSpawner>();

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

    // =====================================
    // 🔁 RESET TRAS MORIR
    // =====================================

    public void ResetDayState(int day)
    {
        Debug.Log("🔁 Reiniciando día");

        // Restaurar día
        currentDay = day;

        // Reiniciar tiempo
        currentTime = 0f;

        // Reiniciar fase
        currentPhase = Phase.Farming;

        // Reactivar farming
        OnFarmingStart?.Invoke();
    }

    // =====================================
    // GETTERS
    // =====================================

    public float GetRemainingTime()
    {
        return dayDuration - currentTime;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    // =====================================
    // FORZAR FIN DEL DÍA
    // =====================================

    public void ForceEndDay()
    {
        // Evitar bugs durante rollback
        if (SaveSystem.Instance != null &&
            SaveSystem.Instance.isRollingBack)
        {
            return;
        }

        // Solo permitir durante defensa
        if (currentPhase != Phase.Defense)
        {
            return;
        }

        EndDay();
    }
}