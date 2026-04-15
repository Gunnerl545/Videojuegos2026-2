using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap tilemap;

    [Header("Tiles")]
    public TileBase grassTile;
    public TileBase plowedTile;
    public TileBase wateredTile;
    public TileBase seededTile;
    public TileBase grownTile;

    private Dictionary<Vector3Int, TileData> tileDataDict = new Dictionary<Vector3Int, TileData>();

    // Obtener posición del tile
    public Vector3Int GetTilePosition(Vector2 worldPosition)
    {
        return tilemap.WorldToCell(worldPosition);
    }

    // Obtener o crear datos del tile
    public TileData GetTileData(Vector3Int pos)
    {
        if (!tileDataDict.ContainsKey(pos))
        {
            tileDataDict[pos] = new TileData();
        }

        return tileDataDict[pos];
    }

    // ⛏️ Arar
    public void HoeTile(Vector3Int position)
    {
        TileBase currentTile = tilemap.GetTile(position);
        TileData data = GetTileData(position);

        if (currentTile == grassTile)
        {
            data.isPlowed = true;
            data.isWatered = false;

            tilemap.SetTile(position, plowedTile);
        }
    }

    public void PlantTile(Vector3Int position)
    {
        TileData data = GetTileData(position);

        if (data.isPlowed && data.isWatered && !data.hasSeed)
        {
            data.hasSeed = true;
            data.growStartTime = Time.time;
            data.isReadyToHarvest = false;

            tilemap.SetTile(position, seededTile);
        }
    }

    // 💧 Regar
    public void WaterTile(Vector3Int position)
    {
        TileData data = GetTileData(position);

        if (data.isPlowed && !data.isWatered)
        {
            data.isWatered = true;

            tilemap.SetTile(position, wateredTile);
        }
    }

    public float growDuration = 10f; // temporal (luego será 1 día)

    void Update()
    {
        UpdateGrowth();
    }

    void UpdateGrowth()
    {
        foreach (var pair in tileDataDict)
        {
            TileData data = pair.Value;

            if (data.hasSeed && !data.isReadyToHarvest)
            {
                if (Time.time - data.growStartTime >= growDuration)
                {
                    data.isReadyToHarvest = true;

                    tilemap.SetTile(pair.Key, grownTile);
                }
            }
        }
    }

    public bool HarvestTile(Vector3Int position)
    {
        TileData data = GetTileData(position);

        if (data.hasSeed && data.isReadyToHarvest)
        {
            // Reset estado (pero dejamos la tierra arada)
            data.hasSeed = false;
            data.isWatered = false;
            data.isReadyToHarvest = false;

            // Volver a tierra arada
            tilemap.SetTile(position, plowedTile);

            return true; // éxito (para dar recompensa)
        }

        return false;
    }
}