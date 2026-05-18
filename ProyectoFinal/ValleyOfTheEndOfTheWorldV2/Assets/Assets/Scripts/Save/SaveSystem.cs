using UnityEngine;
using System.Collections.Generic;

// Sistema encargado de guardar y restaurar el estado completo del juego
public class SaveSystem : MonoBehaviour
{
    // Singleton global
    public static SaveSystem Instance;

    // Evita eventos durante rollback
    public bool isRollingBack = false;

    // Snapshot guardado
    private GameState savedState;

    // Referencias principales
    private GridManager gridManager;
    private PlayerFarming playerFarming;
    private Transform player;
    private TimeManager timeManager;

    void Awake()
    {
        // Inicializar singleton
        Instance = this;

        // Buscar referencias
        FindReferences();
    }

    // Buscar referencias importantes
    void FindReferences()
    {
        gridManager = FindObjectOfType<GridManager>();

        playerFarming = FindObjectOfType<PlayerFarming>();

        timeManager = FindObjectOfType<TimeManager>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    // 💾 GUARDAR JUEGO
    public void SaveGame(int currentDay)
    {
        // Reintentar referencias si falta algo
        if (gridManager == null ||
            playerFarming == null ||
            player == null ||
            timeManager == null)
        {
            Debug.LogWarning("Reintentando referencias...");

            FindReferences();
        }

        // Verificar referencias
        if (gridManager == null ||
            playerFarming == null ||
            player == null ||
            timeManager == null)
        {
            Debug.LogError("No se pudo guardar el juego");
            return;
        }

        // Crear snapshot nuevo
        savedState = new GameState();

        // Guardar día
        savedState.day = currentDay;

        // Guardar posición jugador
        savedState.playerPosition =
            player.position;

        // Guardar trigo
        savedState.wheatCount =
            playerFarming.wheatCount;

        // Guardar grid completo
        savedState.gridData =
            DeepCopyGrid(gridManager.GetGridData());

        Debug.Log("💾 Juego guardado correctamente");
    }

    // 🔁 CARGAR JUEGO
    public void LoadGame()
    {
        // ACTIVAR rollback
        isRollingBack = true;

        // Verificar save
        if (savedState == null)
        {
            Debug.LogError("No hay estado guardado");

            isRollingBack = false;
            return;
        }

        // Verificar grid
        if (savedState.gridData == null)
        {
            Debug.LogError("GridData es null");

            isRollingBack = false;
            return;
        }

        // Restaurar grid
        gridManager.SetGridData(savedState.gridData);

        // Restaurar posición
        player.position =
            savedState.playerPosition;

        // Restaurar trigo
        playerFarming.wheatCount =
            savedState.wheatCount;

        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.RestoreFullHealth();
        }

        // Reiniciar día correctamente
        timeManager.ResetDayState(savedState.day);

        Debug.Log("🔁 Rollback completo aplicado");

        // DESACTIVAR rollback
        isRollingBack = false;
    }

    // 🧠 COPIA PROFUNDA DEL GRID
    private Dictionary<Vector3Int, TileData> DeepCopyGrid(
        Dictionary<Vector3Int, TileData> original)
    {
        Dictionary<Vector3Int, TileData> copy =
            new Dictionary<Vector3Int, TileData>();

        foreach (var pair in original)
        {
            TileData newTile = new TileData
            {
                isPlowed = pair.Value.isPlowed,
                isWatered = pair.Value.isWatered,
                hasSeed = pair.Value.hasSeed,
                growStartTime = pair.Value.growStartTime,
                isReadyToHarvest = pair.Value.isReadyToHarvest
            };

            copy[pair.Key] = newTile;
        }

        return copy;
    }
}