using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class EndlessTerrain : MonoBehaviour
{

    public int meshResolutionFactor = 4;

    public const float maxViewDst = 4500;
    public Transform viewer;

    public static Vector2 viewerPosition;
    int chunkSize;
    int chunksVisibleInViewDst;
    public Vector2 myVector;

    public int widthOfTile;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();
    public static Queue<TileData> dataQueue = new Queue<TileData>();
    MapGenerator mapGen;

    public static string[] tablesArray;

    private async Task<string[]> GetTablesAsync()
    {
        return await LoadSQLData.GetTables();
    }

    private async void RequestTablesAsync()
    {
        try
        {
            var result = await GetTablesAsync();
            tablesArray = result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    void Start()
    {
        chunkSize = 2048;
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
        myVector = new Vector3(0.5f, 0);
        mapGen = FindObjectOfType<MapGenerator>();

        // Populate the tables array
        RequestTablesAsync();

    }

    private async Task<DataCapsule> RequestData(string tileName, int size, int lod)
    {
        var _client = new LoadSQLData(tileName, size, lod);
        return await _client.GetData();
    }

    public static float ParseFloat(string aStr, float aDefault = 0f)
    {
        float v;
        if (float.TryParse(aStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out v))
            return v;
        return aDefault;
    }

    private async void RequestDataAsync(string dataTile)
    {
        try
        {
            dynamic result = await RequestData(dataTile, (widthOfTile + 1) * (widthOfTile + 1), meshResolutionFactor);
            string[] dataSplit = result.data.ToString().Split(new char[] { ',' });
            string tileName = result.tileName;
            int localY = 0;
            int width = (int)Math.Sqrt(dataSplit.Length);
            float[,] heightMap = new float[width, width];



            Debug.Log("Length: " + dataSplit.Length);
            Debug.Log("Length: last value " + dataSplit[dataSplit.Length-1]);
            Debug.Log("width: " + width);

            for (int i = 0; i < dataSplit.Length-1; i++)
            {
                if (i % width == 0 && i != 0)
                {
                    localY++;
                }    
                heightMap[i - (localY * width), localY] = ParseFloat(dataSplit[i]);

            }

            dataQueue.Enqueue(new TileData(tileName, heightMap, meshResolutionFactor));

        }
        catch(Exception ex)
        {
            throw ex;
        }
    }


    void Update()
    {
        var thisV = new Vector2(viewer.position.x + 20f, 0);
        viewer.position = thisV;
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Space has been pressed");
            string[] tablesTestArray = { "_x1_y5", "_x1_y6", "_x1_y4", "_x0_y4" };

            // Here comes the killshot
            foreach (string dataTile in tablesArray)
            {
                RequestDataAsync(dataTile);
            }

        }

        if (dataQueue.Count > 0 )
        {
            for (int i = 0; i < dataQueue.Count; i++)
            {
                mapGen.GenerateMap(dataQueue.Dequeue());

            }
        }
    }

    void UpdateVisibleChunks()
    {

        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
                    }
                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform));
                }

            }
        }
    }

    public class TerrainChunk
    {

        GameObject meshObject;
        Vector2 position;
        Bounds bounds;


        public TerrainChunk(Vector2 coord, int size, Transform parent)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one * size / 10f;
            meshObject.transform.parent = parent;
            SetVisible(false);
        }

        public void UpdateTerrainChunk()
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDstFromNearestEdge <= maxViewDst;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }

    }
}