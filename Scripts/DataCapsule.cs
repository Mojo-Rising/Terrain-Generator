using System.Collections;
using System.Collections.Generic;


public struct DataCapsule
{
    public readonly string tileName;
    public readonly string data;

    public DataCapsule(string _tileName, string _data)
    {
        tileName = _tileName;
        data = _data;
    }
}
