using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
    public int day;

    public Vector3 playerPosition;
    public int wheatCount;

    public Dictionary<Vector3Int, TileData> gridData;
}