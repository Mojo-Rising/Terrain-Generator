using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSlice
{
    public int sliceX;
    public int sliceY;
    public float[,] sliceHeightMap;

    public TileSlice(float[,] heightMap, int x, int y)
    {
        sliceHeightMap = heightMap;
        sliceX = x;
        sliceY = y;
    }

    public static List<TileSlice> GetSlicedHeightMap(int divider, float[,] heightMap, int meshResolutionFactor)
    {
        int w = heightMap.GetLength(0) - 1;
        int h = heightMap.GetLength(1) - 1;
        int chunkSize = (w * h) / (divider * divider);
        int numberOfChunks = (w * h) / chunkSize;
        float chunkRoot = Mathf.Sqrt(chunkSize);

        List<TileSlice> slicedChunks = new List<TileSlice>();

        int xOff = 0;
        int yOff = 0;

        for (int i = 0; i < numberOfChunks; i++)
        {
            float[,] thisSliceHeightMap = new float[(int)chunkRoot+1, (int)chunkRoot+1];

            if (xOff == divider)
            {
                xOff = 0;
                yOff++;
            }

            for (int y = 0; y < chunkRoot+1; y++)
            {
                for (int x = 0; x < chunkRoot+1; x++)
                {
                    thisSliceHeightMap[x, y] = heightMap[x + (xOff * (int)chunkRoot), y + (yOff * (int)chunkRoot)];
                }
            }

            TileSlice thisSlice = new TileSlice(thisSliceHeightMap, (xOff * (int)chunkRoot), -(yOff * (int)chunkRoot));
            slicedChunks.Add(thisSlice);
            xOff++;

        }

        return slicedChunks;

    }

}



