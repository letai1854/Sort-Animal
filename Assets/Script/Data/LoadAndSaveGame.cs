using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class LoadAndSaveGame :SingletonMonoBehaviour<LoadAndSaveGame>   
{
    public string GetJsonData(string file_path)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(file_path);
        if (jsonFile == null)
        {
            Debug.LogError($"Could not load level data file: Resources/.json (or without extension)");
        }
        string jsonData = jsonFile.text;
        return jsonData;
    }
    public bool SaveData(string content,string pathFile)
    {
        try
        {

            string folderPath = Path.Combine(Application.dataPath, CONST.RESOURCES);
            string filePath = Path.Combine(folderPath, pathFile);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            File.WriteAllText(filePath, content);

            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving level ID: {ex.Message}");
            return false;
        }

    }
}
