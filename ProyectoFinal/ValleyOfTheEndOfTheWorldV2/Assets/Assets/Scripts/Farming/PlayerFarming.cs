using UnityEngine;

// Maneja todas las acciones de farming del jugador
public class PlayerFarming : MonoBehaviour
{
    // Distancia a la que el jugador puede interactuar
    public float interactionDistance = 1f;

    // Recurso recolectado (trigo)
    public int wheatCount = 0;

    // Controla si el jugador puede farmear (depende de la fase del juego)
    private bool canFarm = true;

    // Referencia al sistema del grid (mundo)
    public GridManager gridManager; // ASIGNAR EN UNITY

    // Referencia al controlador del jugador
    private PlayerController player;

    void Start()
    {
        // Obtener referencia al PlayerController
        player = GetComponent<PlayerController>();

        // Validación de seguridad
        if (player == null)
        {
            Debug.LogError("PlayerController no encontrado");
        }

        if (gridManager == null)
        {
            Debug.LogError("GridManager no asignado en el inspector");
        }
    }

    // Suscripción a eventos del TimeManager
    void OnEnable()
    {
        TimeManager.OnDefenseStart += DisableFarming;
        TimeManager.OnFarmingStart += EnableFarming;
    }

    void OnDisable()
    {
        TimeManager.OnDefenseStart -= DisableFarming;
        TimeManager.OnFarmingStart -= EnableFarming;
    }

    // Desactiva farming en fase de defensa
    void DisableFarming()
    {
        canFarm = false;
    }

    // Activa farming en fase de farming
    void EnableFarming()
    {
        canFarm = true;
    }

    void Update()
    {
        // Si no puede farmear, no hace nada
        if (!canFarm) return;

        // Input de acciones
        if (Input.GetKeyDown(KeyCode.Space))
            Hoe();     // arar

        if (Input.GetMouseButtonDown(1))
            Water();   // regar

        if (Input.GetKeyDown(KeyCode.F))
            Plant();   // sembrar

        if (Input.GetKeyDown(KeyCode.E))
            Harvest(); // cosechar
    }

    // Arar tierra
    void Hoe()
    {
        Vector2 point = player.GetInteractionPoint(interactionDistance);
        Vector3Int tilePos = gridManager.GetTilePosition(point);

        gridManager.HoeTile(tilePos);
    }

    // Regar tierra
    void Water()
    {
        Vector2 point = player.GetInteractionPoint(interactionDistance);
        Vector3Int tilePos = gridManager.GetTilePosition(point);

        gridManager.WaterTile(tilePos);
    }

    // Sembrar
    void Plant()
    {
        Vector2 point = player.GetInteractionPoint(interactionDistance);
        Vector3Int tilePos = gridManager.GetTilePosition(point);

        gridManager.PlantTile(tilePos);
    }

    // Cosechar
    void Harvest()
    {
        Vector2 point = player.GetInteractionPoint(interactionDistance);
        Vector3Int tilePos = gridManager.GetTilePosition(point);

        bool success = gridManager.HarvestTile(tilePos);

        // Si se pudo cosechar, aumentar recurso
        if (success)
        {
            wheatCount++;
            Debug.Log("Trigo: " + wheatCount);
        }
    }
}