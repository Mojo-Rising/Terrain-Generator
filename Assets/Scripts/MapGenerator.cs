using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;

public class MapGenerator : MonoBehaviour
{

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public bool autoUpdate;


    public int divider = 16;
    public bool sliceTile;

    public float meshHeightMultiplier = 200;
    [Range(0, 3)]
    public int levelOfDetail;

    public enum DrawMode { HeightMap, Mesh };
    public DrawMode drawMode;

    public int tilesToLoad = 10;
    public int tilesLoaded = 0;

    public static int tileCount;

    


    public void GenerateMap(TileData currentTile)
    {
        Debug.Log("MapGen triggered with tile: " + currentTile.tileName);

        // Get the MapDisplay script reference
        MapDisplay display = FindObjectOfType<MapDisplay>();

        // tileCount is used to determine how many tiles have been generated
        tileCount = 0;

        string tileName = currentTile.tileName;
        float[,] heightMap = currentTile.heightMap;
        int meshResolutionFactor = 4;
        int width = heightMap.GetLength(0);
        

        // generate the parent coordinates for the tile position. The position is extracted from the filename with the GetTilePositionFromString function
        Vector3 tileCoord = new Vector3();
        tileCoord = TileLocationReader.GetTilePositionFromStringTileData(tileName);
        Debug.Log("position: " + tileCoord.ToString() + ", tile: " + tileName);
        Vector3 tilePosition = new Vector3(tileCoord.x * (width -1), 0, tileCoord.z * (width -1) * -1);

        // Create the parent tile game object and set it
        GameObject tile = new GameObject();
        tile.name = tileName;

        // Tiles are sliced to accomodate the 65k vertices limit for meshes. Slicing and the slice amount are determined in the editor. Default is on and 16 by 16 pieces. Must use quadratic numbers
        if (sliceTile)
        {
            // The list is filled with the TileSlice class, which houses the sliced heightmap and the local position
            List<TileSlice> slicedChunks = new List<TileSlice>();
            slicedChunks = TileSlice.GetSlicedHeightMap(divider, heightMap, meshResolutionFactor);

            // Loop over the list of slices and draw them at their local position
            foreach (TileSlice thisSlice in slicedChunks)
            {
                Vector3 position = new Vector3(thisSlice.sliceX, 0, thisSlice.sliceY);

                if (drawMode == DrawMode.Mesh)
                {
                    display.DrawMesh(MeshGenerator.GenerateTerrainMesh(thisSlice.sliceHeightMap, meshHeightMultiplier, levelOfDetail, meshResolutionFactor), position, tile.transform);
                }
                else if (drawMode == DrawMode.HeightMap)
                {
                    display.DrawNoiseMap(thisSlice.sliceHeightMap, position, tile.transform, tileCoord);
                }

                tileCount++;
            }
        }
        // In case of no slicing, draw map on the global position. Do not attempt to draw the mesh without slicing!
        else
        {
            tilePosition = new Vector3(tilePosition.x * -1, 0, tilePosition.z);
            display.DrawNoiseMap(heightMap, Vector3.zero, tile.transform, tileCoord);
            tileCount++;
        }

        tile.transform.position = tilePosition;

        tilesLoaded++;

    }

}
