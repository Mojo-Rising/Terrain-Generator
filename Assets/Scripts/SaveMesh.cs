using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveMesh : MonoBehaviour {

    public static void SaveAsHeightMap(Texture2D tex, Vector3 coord, int width)
    {
        byte[] bytes = tex.EncodeToPNG();

        File.WriteAllBytes(Application.dataPath + "/../HeightMaps/heightMap_x" + coord.x + "_y" + coord.z + ".png", bytes);
    }
}
