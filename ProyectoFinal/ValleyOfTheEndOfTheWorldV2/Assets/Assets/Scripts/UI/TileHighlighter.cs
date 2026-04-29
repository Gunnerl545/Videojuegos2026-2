using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHighlighter : MonoBehaviour
{
    public Tilemap tilemap;
    public PlayerController player;

    public float interactionDistance = 1f;

    void Update()
    {
        UpdateHighlight();
    }

    void UpdateHighlight()
    {
        // 1. Obtener punto frente al jugador
        Vector2 interactionPoint = player.GetInteractionPoint(interactionDistance);

        // 2. Convertir a celda
        Vector3Int cellPosition = tilemap.WorldToCell(interactionPoint);

        // 3. Obtener centro del tile
        Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPosition);

        // 4. Mover highlight
        transform.position = cellCenter;
    }
}