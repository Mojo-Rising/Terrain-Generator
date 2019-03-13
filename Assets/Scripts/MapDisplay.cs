using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapDisplay : MonoBehaviour {

    public bool saveHeightMaps;

    public void DrawNoiseMap(float[,] noiseMap, Vector3 position, Transform parentTile, Vector3 coord) {

        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Renderer textureRender = plane.GetComponent(typeof(MeshRenderer)) as MeshRenderer;


		int width = noiseMap.GetLength (0);
		int height = noiseMap.GetLength (1);

        Texture2D texture = new Texture2D (width, height);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, noiseMap [x, y]);
			}
		}

		texture.SetPixels (colourMap);
		texture.Apply ();

        string materialName = "SliceMaterial" + MapGenerator.tileCount.ToString();
        Material newMaterial = CreateMaterial(texture, materialName);

        //textureRender.sharedMaterial.shader = Shader.Find("Unlit/Texture");
        //textureRender.sharedMaterial.mainTexture = texture;
        textureRender.material = newMaterial;
        texture.wrapMode = TextureWrapMode.Clamp;
        newMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3 ((float)width/10, 1, (float)height/10);
        plane.transform.parent = parentTile.transform;
        plane.transform.localPosition = position;

        if (saveHeightMaps)
        {
            SaveMesh.SaveAsHeightMap(texture, coord, width);
            Debug.Log(coord);
        }

        MapGenerator.tileCount++;

    }

    static Material CreateMaterial(Texture2D texture, string materialName)
    {
        // Create a simple material asset
        if (System.IO.File.Exists("Assets/Materials/" + materialName + ".mat"))
        {
            Material material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/" + materialName + ".mat", typeof(Material));

            return material;
        } else
        {
            Material material = new Material(Shader.Find("Unlit/Texture"));
            AssetDatabase.CreateAsset(material, "Assets/Materials/" + materialName + ".mat");
            AssetDatabase.Refresh();

            return material;
        }

        
    }

    public void DrawMesh(MeshData meshData, Vector3 position, Transform parentTile)
    {
        GameObject mesh = new GameObject();
        mesh.name = "TileSlice" + MapGenerator.tileCount.ToString();
        MeshFilter meshFilter = mesh.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = mesh.AddComponent<MeshRenderer>();
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/" + "Map Mat.mat", typeof(Material));

        position = new Vector3(position.x, position.y, position.z);       
        mesh.transform.parent = parentTile;
        mesh.transform.localPosition = position;

    }

}
