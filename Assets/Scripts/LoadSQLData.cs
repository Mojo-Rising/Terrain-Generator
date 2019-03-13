using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

class LoadSQLData : System.Net.WebClient {

    private readonly string m_Uri = "http://127.0.0.1/test/LoadDataFromSQL.php";
    private readonly string m_TileName;
    private readonly int m_squaredWidth;
    private readonly int m_meshResolutionFactor;

    public LoadSQLData(string tileName, int squaredWidth, int lod)
    {
        m_TileName = tileName;
        m_squaredWidth = squaredWidth;
        m_meshResolutionFactor = lod;
    }

    public static async Task<string[]> GetTables()
    {
        string url = "http://127.0.0.1/test/GetTablesInDatabase.php";
        var client = new System.Net.WebClient();
        string response = await client.DownloadStringTaskAsync(url);
        string[] dataSplit = response.Split(new char[] { ',' });

        return dataSplit;
    }

    public async Task<DataCapsule> GetData()
    {
        var data = new NameValueCollection
        {
            {"tile", m_TileName },
            {"size", m_squaredWidth.ToString() },
            {"lod", m_meshResolutionFactor.ToString() }
        };
        byte[] response = await UploadValuesTaskAsync(new Uri(m_Uri), data);
        string responseStr = Encoding.ASCII.GetString(response);

        return new DataCapsule(m_TileName, responseStr);
    }

    
}
