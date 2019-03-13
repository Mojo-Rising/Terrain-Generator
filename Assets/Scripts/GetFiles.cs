using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class GetFiles {

    public static List<string> GetFilesList(string filePath)
    {        
        DirectoryInfo tilesFolderInfo = new DirectoryInfo(filePath);

        List<string> fileNames = new List<string>();

        foreach (FileInfo fi in tilesFolderInfo.GetFiles())
        {
            if (!fi.Name.EndsWith(".meta") && fi.Length != 67178607 && fi.Length != 67178606)
            {
                fileNames.Add(fi.Name);
            }
            
        }

        return fileNames;
    }

}


