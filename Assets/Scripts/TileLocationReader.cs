using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public static class TileLocationReader
{

    public static Vector3 GetTilePositionFromString(string fileName)
    {
        char[] charArray = fileName.ToCharArray();
        Vector3 position = new Vector3();
        position.y = 0;

        for (int i = 0; i < charArray.Length; i++)
        {

            if (charArray[i] == '_')
            {

                if (Char.IsDigit(charArray[i - 2]))
                {
                    char[] ab = { charArray[i - 2], charArray[i - 1] };
                    string dc = new string(ab);

                    position.x = Int32.Parse(dc);
                }
                else
                {
                    char[] a = { charArray[i - 1] };
                    string b = new string(a);

                    position.x = Int32.Parse(b);
                }

                if (Char.IsDigit(charArray[i + 2]))
                {
                    char[] ab = { charArray[i + 1], charArray[i + 2] };
                    string dc = new string(ab);

                    position.z = Int32.Parse(dc);
                }
                else
                {
                    char[] a = { charArray[i + 1] };
                    string b = new string(a);

                    position.z = Int32.Parse(b);
                }

            }
        }

        return position;
    }

    public static Vector3 GetTilePositionFromStringTileData(string tileName)
    {
        char[] charArray = tileName.ToCharArray();
        Vector3 position = new Vector3();
        position.y = 0;

        for (int i = 0; i < charArray.Length; i++)
        {

            if (charArray[i] == 'x')
            {
                if (Char.IsDigit(charArray[i + 2]))
                {
                    char[] ab = { charArray[i + 1], charArray[i + 2] };
                    string dc = new string(ab);

                    position.z = Int32.Parse(dc);
                }
                else
                {
                    char[] a = { charArray[i + 1] };
                    string b = new string(a);

                    position.z = Int32.Parse(b);
                }
            }
            else if (charArray[i] == 'y')
            {

                if (i+2 <= charArray.Length-1)
                {
                    char[] ab = { charArray[i + 1], charArray[i + 2] };
                    string dc = new string(ab);

                    position.x = Int32.Parse(dc);
                }
                else
                {
                    char[] a = { charArray[i + 1] };
                    string b = new string(a);

                    position.x = Int32.Parse(b);
                }

            }
        }

        return position;
    }
}
