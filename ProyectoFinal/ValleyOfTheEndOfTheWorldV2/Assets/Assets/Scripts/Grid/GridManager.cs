using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Controla el estado y comportamiento de todos los tiles del juego
public class GridManager : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap tilemap; // Referencia al tilemap de Unity

    [Header("Tiles")]
    public TileBase grassTile;
    public TileBase plowedTile;
    public TileBase wateredTile;
    public TileBase seededTile;
    public TileBase grownTile;

    // Diccionario que guarda el estado lógico de cada tile
    private Dictionary<Vector3Int, TileData> tileDataDict = new Dictionary<Vector3Int, TileData>();

    // Convierte una posición del mundo a una celda del tilemap
    public Vector3Int GetTilePosition(Vector2 worldPosition)
    {
        return tilemap.WorldToCell(worldPosition);
    }

    // Obtiene el estado del tile o lo crea si no existe
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
        if (tileDataDict == null) return;

        foreach (var pair in tileDataDict)
        {
            if (pair.Value == null) continue;

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

    public Dictionary<Vector3Int, TileData> GetGridData()
    {
        return tileDataDict;
    }

    public void SetGridData(Dictionary<Vector3Int, TileData> newData)
    {
        if (newData == null)
        {
            Debug.LogError("SetGridData recibió null");
            return;
        }

        tileDataDict = newData;

        foreach (var pair in tileDataDict)
        {
            if (pair.Value == null) continue;

            TileData data = pair.Value;

            if (data.isReadyToHarvest)
                tilemap.SetTile(pair.Key, grownTile);
            else if (data.hasSeed)
                tilemap.SetTile(pair.Key, seededTile);
            else if (data.isWatered)
                tilemap.SetTile(pair.Key, wateredTile);
            else if (data.isPlowed)
                tilemap.SetTile(pair.Key, plowedTile);
            else
                tilemap.SetTile(pair.Key, grassTile);
        }
    }
}