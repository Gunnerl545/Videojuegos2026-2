using UnityEngine;

// Maneja todas las las acciones de farming del jugador
public class PlayerFarming : MonoBehaviour
{
    // Distancia a la que el jugador puede interactuar
    public float interactionDistance = 1f;

    // Recurso recolectado (trigo)
    public int wheatCount = 0;

    // Cuánta vida recupera cada trigo
    public int healAmount = 1;

    // Controla si el jugador puede farmear
    private bool canFarm = true;

    // Referencia al sistema del grid
    public GridManager gridManager;

    // Referencia al controlador del jugador
    private PlayerController player;

    // Referencia al sistema de vida
    private PlayerHealth playerHealth;

    void Start()
    {
        // Obtener referencias
        player = GetComponent<PlayerController>();

        playerHealth = GetComponent<PlayerHealth>();

        // Validaciones
        if (player == null)
        {
            Debug.LogError("PlayerController no encontrado");
        }

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth no encontrado");
        }

        if (gridManager == null)
        {
            Debug.LogError("GridManager no asignado");
        }
    }

    // Suscripción a eventos
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

    // Desactivar farming durante defensa
    void DisableFarming()
    {
        canFarm = false;
    }

    // Activar farming durante día
    void EnableFarming()
    {
        canFarm = true;
    }

    void Update()
    {
        // Comer trigo
        if (Input.GetKeyDown(KeyCode.E))
        {
            EatWheat();
        }
    }

    // =========================================
    // ⛏️ ARAR
    // =========================================
    public void Hoe()
    {
        if (!canFarm) return;

        Vector2 point =
            player.GetInteractionPoint(interactionDistance);

        Vector3Int tilePos =
            gridManager.GetTilePosition(point);

        gridManager.HoeTile(tilePos);
    }

    // =========================================
    // 💧 REGAR
    // =========================================
    public void Water()
    {
        if (!canFarm) return;

        Vector2 point =
            player.GetInteractionPoint(interactionDistance);

        Vector3Int tilePos =
            gridManager.GetTilePosition(point);

        gridManager.WaterTile(tilePos);
    }

    // =========================================
    // 🌱 PLANTAR
    // =========================================
    public void Plant()
    {
        if (!canFarm) return;

        Vector2 point =
            player.GetInteractionPoint(interactionDistance);

        Vector3Int tilePos =
            gridManager.GetTilePosition(point);

        gridManager.PlantTile(tilePos);
    }

    // =========================================
    // 🌾 COSECHAR
    // =========================================
    public void Harvest()
    {
        if (!canFarm) return;

        Vector2 point =
            player.GetInteractionPoint(interactionDistance);

        Vector3Int tilePos =
            gridManager.GetTilePosition(point);

        bool success =
            gridManager.HarvestTile(tilePos);

        // Si se pudo cosechar
        if (success)
        {
            wheatCount++;

            Debug.Log("🌾 Trigo actual: " + wheatCount);
        }
    }

    // =========================================
    // 🍞 COMER TRIGO
    // =========================================
    public void EatWheat()
    {
        // Verificar trigo
        if (wheatCount <= 0)
        {
            Debug.Log("❌ No tienes trigo");
            return;
        }

        // Consumir trigo
        wheatCount--;

        // Curar jugador
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
        }

        Debug.Log("🍞 Trigo consumido");
        Debug.Log("🌾 Trigo restante: " + wheatCount);
    }
}