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


    public bool WriteData(string content, string fileName)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            File.WriteAllText(filePath, content);

            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving data to {fileName}: {ex.Message}");
            return false;
        }
    }

    public string ReadData(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error loading data from {fileName}: {ex.Message}");
                return null;
            }
        }
        else
        {
            return null;
        }
    }

}
