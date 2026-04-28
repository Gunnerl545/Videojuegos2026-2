using System;

// Permite que esta clase sea serializable (clave para guardado)
[Serializable]
public class TileData
{
    // Indica si la tierra ha sido arada
    public bool isPlowed;

    // Indica si la tierra ha sido regada
    public bool isWatered;

    // Indica si hay una semilla plantada
    public bool hasSeed;

    // Momento en el que se plantó la semilla
    public float growStartTime;

    // Indica si el cultivo ya está listo para cosechar
    public bool isReadyToHarvest;
}