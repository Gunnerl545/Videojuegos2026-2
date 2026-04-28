using UnityEngine;

// Sistema encargado de guardar y restaurar el estado completo del juego
public class SaveSystem : MonoBehaviour
{
    // Instancia global (patrón Singleton)
    // Permite acceder al SaveSystem desde cualquier script
    public static SaveSystem Instance;

    // Estado guardado en memoria (snapshot del juego)
    private GameState savedState;

    // Referencias a los sistemas principales del juego
    private GridManager gridManager;     // Maneja el mundo (tiles)
    private PlayerFarming playerFarming; // Maneja recursos del jugador (trigo)
    private Transform player;            // Transform del jugador (posición)

    void Awake()
    {
        // Inicialización del Singleton
        // Solo debería existir una instancia en la escena
        Instance = this;
    }

    void Start()
    {
        // Obtener referencias automáticamente al iniciar la escena

        // Encuentra el GridManager en la escena
        gridManager = FindObjectOfType<GridManager>();

        // Encuentra el sistema de farming del jugador
        playerFarming = FindObjectOfType<PlayerFarming>();

        // Busca el objeto con tag "Player" y obtiene su Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // 💾 MÉTODO PRINCIPAL DE GUARDADO
    // Captura el estado actual del juego
    public void SaveGame(int currentDay)
    {
        // Crear un nuevo snapshot (estado limpio)
        savedState = new GameState();

        // Guardar información global
        savedState.day = currentDay;

        // Guardar posición del jugador
        savedState.playerPosition = player.position;

        // Guardar recursos (trigo recolectado)
        savedState.wheatCount = playerFarming.wheatCount;

        // Guardar el estado completo del grid (mundo)
        // IMPORTANTE: se usa copia profunda para evitar referencias compartidas
        savedState.gridData = DeepCopyGrid(gridManager.GetGridData());

        Debug.Log("Juego guardado completo");
    }

    // 🔁 MÉTODO PRINCIPAL DE CARGA (ROLLBACK)
    // Restaura el estado previamente guardado
    public void LoadGame()
    {
        // Validación: no hay estado guardado
        if (savedState == null)
        {
            Debug.LogError("No hay estado guardado");
            return;
        }

        // Validación: el grid guardado es inválido
        if (savedState.gridData == null)
        {
            Debug.LogError("GridData es null");
            return;
        }

        // Restaurar el estado del mundo (tiles)
        gridManager.SetGridData(savedState.gridData);

        // Restaurar posición del jugador
        player.position = savedState.playerPosition;

        // Restaurar recursos del jugador
        playerFarming.wheatCount = savedState.wheatCount;

        Debug.Log("Rollback completo aplicado");
    }

    // 🧠 MÉTODO CRÍTICO: COPIA PROFUNDA
    // Crea una copia independiente del estado del grid
    // Evita que el estado guardado se modifique accidentalmente
    private System.Collections.Generic.Dictionary<Vector3Int, TileData> DeepCopyGrid(
        System.Collections.Generic.Dictionary<Vector3Int, TileData> original)
    {
        // Nuevo diccionario que contendrá la copia
        var copy = new System.Collections.Generic.Dictionary<Vector3Int, TileData>();

        // Recorrer todos los tiles del grid original
        foreach (var pair in original)
        {
            // Crear un nuevo TileData (NO referencia al original)
            TileData newTile = new TileData
            {
                isPlowed = pair.Value.isPlowed,
                isWatered = pair.Value.isWatered,
                hasSeed = pair.Value.hasSeed,
                growStartTime = pair.Value.growStartTime,
                isReadyToHarvest = pair.Value.isReadyToHarvest
            };

            // Guardar la copia en la misma posición
            copy[pair.Key] = newTile;
        }

        // Retornar el nuevo diccionario completamente independiente
        return copy;
    }
}