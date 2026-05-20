using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Controla el estado y comportamiento de todos los tiles del juego
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

    // Diccionario con el estado lógico de los tiles
    private Dictionary<Vector3Int, TileData> tileDataDict =
        new Dictionary<Vector3Int, TileData>();

    // Tiempo de crecimiento
    public float growDuration = 10f;

    // =====================================
    // OBTENER POSICIÓN TILE
    // =====================================

    public Vector3Int GetTilePosition(Vector2 worldPosition)
    {
        return tilemap.WorldToCell(worldPosition);
    }

    // =====================================
    // OBTENER DATOS TILE
    // =====================================

    public TileData GetTileData(Vector3Int pos)
    {
        if (!tileDataDict.ContainsKey(pos))
        {
            tileDataDict[pos] = new TileData();
        }

        return tileDataDict[pos];
    }

    // =====================================
    // ⛏️ ARAR
    // =====================================

    public void HoeTile(Vector3Int position)
    {
        TileBase currentTile =
            tilemap.GetTile(position);

        TileData data =
            GetTileData(position);

        if (currentTile == grassTile)
        {
            data.isPlowed = true;

            data.isWatered = false;

            data.hasSeed = false;

            data.isReadyToHarvest = false;

            tilemap.SetTile(
                position,
                plowedTile);
        }
    }

    // =====================================
    // 💧 REGAR
    // =====================================

    public void WaterTile(Vector3Int position)
    {
        TileData data =
            GetTileData(position);

        if (data.isPlowed &&
            !data.isWatered)
        {
            data.isWatered = true;

            tilemap.SetTile(
                position,
                wateredTile);
        }
    }

    // =====================================
    // 🌱 SEMBRAR
    // =====================================

    public void PlantTile(Vector3Int position)
    {
        TileData data =
            GetTileData(position);

        if (data.isPlowed &&
            data.isWatered &&
            !data.hasSeed)
        {
            data.hasSeed = true;

            data.growStartTime =
                Time.time;

            data.isReadyToHarvest = false;

            tilemap.SetTile(
                position,
                seededTile);
        }
    }

    // =====================================
    // ACTUALIZAR CRECIMIENTO
    // =====================================

    void Update()
    {
        UpdateGrowth();
    }

    void UpdateGrowth()
    {
        if (tileDataDict == null)
        {
            return;
        }

        foreach (var pair in tileDataDict)
        {
            if (pair.Value == null)
            {
                continue;
            }

            TileData data = pair.Value;

            // Verificar crecimiento
            if (data.hasSeed &&
                !data.isReadyToHarvest)
            {
                if (Time.time - data.growStartTime
                    >= growDuration)
                {
                    data.isReadyToHarvest = true;

                    tilemap.SetTile(
                        pair.Key,
                        grownTile);
                }
            }
        }
    }

    // =====================================
    // 🌾 COSECHAR
    // =====================================

    public bool HarvestTile(Vector3Int position)
    {
        TileData data =
            GetTileData(position);

        if (data.hasSeed &&
            data.isReadyToHarvest)
        {
            // Reset estado
            data.hasSeed = false;

            data.isWatered = false;

            data.isReadyToHarvest = false;

            // Mantener tierra arada
            tilemap.SetTile(
                position,
                plowedTile);

            return true;
        }

        return false;
    }

    // =====================================
    // OBTENER GRID
    // =====================================

    public Dictionary<Vector3Int, TileData> GetGridData()
    {
        return tileDataDict;
    }

    // =====================================
    // RESTAURAR GRID
    // =====================================

    public void SetGridData(
        Dictionary<Vector3Int, TileData> newData)
    {
        if (newData == null)
        {
            Debug.LogError(
                "SetGridData recibió null");

            return;
        }

        // =====================================
        // LIMPIAR TILES ACTUALES
        // =====================================

        foreach (var pair in tileDataDict)
        {
            tilemap.SetTile(
                pair.Key,
                grassTile);
        }

        // =====================================
        // NUEVO DICCIONARIO
        // =====================================

        tileDataDict =
            new Dictionary<Vector3Int, TileData>();

        // =====================================
        // COPIA PROFUNDA
        // =====================================

        foreach (var pair in newData)
        {
            if (pair.Value == null)
            {
                continue;
            }

            TileData copiedTile =
                new TileData
            {
                isPlowed =
                    pair.Value.isPlowed,

                isWatered =
                    pair.Value.isWatered,

                hasSeed =
                    pair.Value.hasSeed,

                growStartTime =
                    pair.Value.growStartTime,

                isReadyToHarvest =
                    pair.Value.isReadyToHarvest
            };

            tileDataDict[pair.Key] =
                copiedTile;
        }

        // =====================================
        // RECONSTRUIR GRID
        // =====================================

        foreach (var pair in tileDataDict)
        {
            TileData data = pair.Value;

            // Planta crecida
            if (data.isReadyToHarvest)
            {
                tilemap.SetTile(
                    pair.Key,
                    grownTile);
            }

            // Planta sembrada
            else if (data.hasSeed)
            {
                tilemap.SetTile(
                    pair.Key,
                    seededTile);
            }

            // Tierra regada
            else if (data.isWatered)
            {
                tilemap.SetTile(
                    pair.Key,
                    wateredTile);
            }

            // Tierra arada
            else if (data.isPlowed)
            {
                tilemap.SetTile(
                    pair.Key,
                    plowedTile);
            }

            // Pasto
            else
            {
                tilemap.SetTile(
                    pair.Key,
                    grassTile);
            }
        }

        Debug.Log(
            "🌱 Grid restaurado correctamente");
    }
}