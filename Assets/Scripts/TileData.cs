using System.Collections;
using System.Collections.Generic;

public struct TileData {

    public readonly string tileName;
    public readonly float[,] heightMap;
    public readonly int resolutionFactor;

    public TileData(string name, float[,] map, int resolution)
    {
        tileName = name;
        heightMap = map;
        resolutionFactor = resolution;
    }
}
