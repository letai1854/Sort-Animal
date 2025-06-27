using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class PlayerDataService : MonoBehaviour
{
 


    public bool SavePlayerSettings(PlayerSettings settings, string fileName)
    {
        try
        {
            string folderPath = Path.Combine(Application.dataPath, CONST.RESOURCES);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(filePath, json);

            Debug.Log("Player settings saved to: " + filePath);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving player settings: {ex.Message}");
            return false;
        }
    }
    public bool UpdatePlayerSetting<T>(string fileName, System.Action<PlayerSettings> updateAction)
    {
        try
        {
            string filePath = Path.Combine(Application.dataPath, CONST.RESOURCES, fileName);

            PlayerSettings settings;

            // Load old data if exists, or create new
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                settings = JsonConvert.DeserializeObject<PlayerSettings>(json);
            }
            else
            {
                settings = new PlayerSettings();
            }

            // Apply the specific update
            updateAction.Invoke(settings);

            // Save back
            string updatedJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);

            Debug.Log("Player settings updated: " + filePath);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error updating player setting: {ex.Message}");
            return false;
        }
    }
    //    LoadAndSaveGame.Instance.UpdatePlayerSetting("PlayerSettings.json", (settings) =>
    //{
    //    settings.coins = "999999";
    //});
    public PlayerSettings LoadPlayerSettings(string fileName)
    {
        try
        {
            string json = LoadAndSaveGame.Instance. GetJsonData(fileName);
            PlayerSettings settings = JsonConvert.DeserializeObject<PlayerSettings>(json);
            return settings;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading player settings: {ex.Message}");
            return null;
        }
    }
}
