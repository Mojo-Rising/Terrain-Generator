using System;
using System.IO;
using Newtonsoft.Json;

[Serializable]
public class Tile
{
    public float[,] Tiles;

    public int X { get; set; }
    public int Y { get; set; }

    public int RealX { get; set; }
    public int RealY { get; set; }
    public int RealSize { get; set; }

    public int SizeX
    {
        get
        {
            return Tiles.GetLength(0);
        }
    }
    public int SizeY
    {
        get
        {
            return Tiles.GetLength(1);
        }
    }

    /// <summary>
    /// Creates a 2D array of floats, that are clamped between 0 and 1.
    /// </summary>
    public float[,] GetHeightMap(int minimum, int maximum)
    {
        //Calculate the float, given the maximum and minimum.
        float[,] map = new float[SizeX, SizeY];
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                float f = Tiles[x, y] - minimum;
                float ff = f / maximum;
                if (ff < 0)
                    ff = 0;
                if (ff > 1)
                    ff = 1;
                map[x, y] = ff;
            }
        }

        
        return map;
    }

    public Tile(int size_x, int size_y, int real_x, int real_y)
    {
        RealX = real_x;
        RealY = real_y;

        Tiles = new float[size_x, size_y];
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                Tiles[x, y] = float.MinValue;
            }
        }
    }

    public Tile()
    {

    }

    public static Tile LoadTile(string path)
    {
        return JsonConvert.DeserializeObject<Tile>(File.ReadAllText(path));
    }
}