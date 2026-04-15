using UnityEngine;

public class PlayerFarming : MonoBehaviour
{
    public float interactionDistance = 1f;
    public int wheatCount = 0;

    public GridManager gridManager; // ASIGNAR EN UNITY

    private PlayerController player;

    void Start()
    {
        player = GetComponent<PlayerController>();

        if (player == null)
        {
            Debug.LogError("PlayerController no encontrado");
        }

        if (gridManager == null)
        {
            Debug.LogError("GridManager no asignado en el inspector");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hoe();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Water();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Plant();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Harvest();
        }
    }

    void Hoe()
    {
        Vector2 point = player.GetInteractionPoint(interactionDistance);
        Vector3Int tilePos = gridManager.GetTilePosition(point);

        gridManager.HoeTile(tilePos);
    }

    void Water()
    {
        Vector2 point = player.GetInteractionPoint(interactionDistance);
        Vector3Int tilePos = gridManager.GetTilePosition(point);

        gridManager.WaterTile(tilePos);
    }

    void Plant()
    {
        Vector2 point = player.GetInteractionPoint(interactionDistance);
        Vector3Int tilePos = gridManager.GetTilePosition(point);

        gridManager.PlantTile(tilePos);
    }

    void Harvest()
    {
        Vector2 point = player.GetInteractionPoint(interactionDistance);
        Vector3Int tilePos = gridManager.GetTilePosition(point);

        bool success = gridManager.HarvestTile(tilePos);

        if (success)
        {
            wheatCount++;
            Debug.Log("Trigo: " + wheatCount);
        }
    }
}