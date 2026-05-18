using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
    // Día actual
    public int day;

    // Posición del jugador
    public Vector3 playerPosition;

    // Recursos
    public int wheatCount;

    // Estado completo del grid
    public Dictionary<Vector3Int, TileData> gridData;

    // NUEVO → tiempo actual del día
    public float currentTime;

    // NUEVO → fase actual del juego
    public TimeManager.Phase currentPhase;
}