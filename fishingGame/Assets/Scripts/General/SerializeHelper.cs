//Helps serialize classes and store relevant information in disk

using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class SerializeHelper
{
    /// <summary>
    /// Serialize the given class in the specified path with the given file name.
    /// </summary>
    public void StoreData<T>(string full_path, string file_name, T class_raw)
    {
        //Create directory for storage if it does not exist
        System.IO.Directory.CreateDirectory(full_path);
        
        string json = JsonUtility.ToJson(class_raw, true);
        System.IO.File.WriteAllText(full_path + file_name + ".json", json);
    }

    /// <summary>
    /// Retrieves the serialzed data from the specified path and overwrites the given class data.
    /// </summary>
    public void RetrieveData<T>(string full_path, string file_name, T class_raw)
    {
        //Create directory for saves storage if it does not exist
        System.IO.Directory.CreateDirectory(full_path);

        if (!File.Exists(full_path + "/" + file_name + ".json"))
        {
            Debug.LogError(full_path + "/" + file_name + ".json was not found.");
            return;
        }

        string json = System.IO.File.ReadAllText(full_path + "/" + file_name + ".json");
        JsonUtility.FromJsonOverwrite(json, class_raw);
    }

}
