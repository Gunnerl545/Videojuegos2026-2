using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    [Header("Duración (segundos)")]
    public float dayDuration = 1500f;     // 25 min
    public float farmingDuration = 1200f; // 20 min

    private float currentTime = 0f;

    public enum Phase
    {
        Farming,
        Defense
    }

    public Phase currentPhase;

    public int currentDay = 1;

    // Eventos (CLAVE para escalar)
    public static event Action OnFarmingStart;
    public static event Action OnDefenseStart;
    public static event Action OnDayEnd;

    void Start()
    {
        currentPhase = Phase.Farming;
        OnFarmingStart?.Invoke();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        CheckPhaseChange();
        CheckDayEnd();
    }

    void CheckPhaseChange()
    {
        if (currentPhase == Phase.Farming && currentTime >= farmingDuration)
        {
            currentPhase = Phase.Defense;
            Debug.Log("🧟 Defensa iniciada");

            OnDefenseStart?.Invoke();
        }
    }

    void CheckDayEnd()
    {
        if (currentTime >= dayDuration)
        {
            EndDay();
        }
    }

    void EndDay()
    {
        Debug.Log("🌅 Día terminado");

        currentDay++;
        currentTime = 0f;
        currentPhase = Phase.Farming;

        OnDayEnd?.Invoke();
        OnFarmingStart?.Invoke();
    }

    // Utilidad
    public float GetRemainingTime()
    {
        return dayDuration - currentTime;
    }
}