using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelDataLoader : SingletonMonoBehaviour<LevelDataLoader>
{

    private int currentLevel=0;
    private AllLevelsData allLevelsDataCache;

    public LevelConfig GetAllLevelsData(string file_path)
    {
        string jsonData = LoadAndSaveGame.Instance.GetJsonData( file_path);
        LevelConfig targetLevelConfig = null;
        if (jsonData!= null)
        {
            int level = GetCurrentLevel();
            AllLevelsData allLevelsData = JsonConvert.DeserializeObject<AllLevelsData>(jsonData);
            allLevelsDataCache = allLevelsData;
            int levelTarget =  CheckLevel(level);
            targetLevelConfig = allLevelsData.levels.FirstOrDefault(lvl => lvl.level == levelTarget);
        }    
        return targetLevelConfig;
    }

    public int GetCurrentLevel()
    {
        string jsonData = LoadAndSaveGame.Instance. GetJsonData(CONST.LEVEL_CURRENT_PATH);
        if (int.TryParse(jsonData, out int level))
        {
            return level;
        }
        return 1;

    }

    //public string GetJsonData(string file_path)
    //{
    //    TextAsset jsonFile = Resources.Load<TextAsset>(file_path);
    //    if (jsonFile == null)
    //    {
    //        Debug.LogError($"Could not load level data file: Resources/.json (or without extension)");
    //    }
    //    string jsonData = jsonFile.text;
    //    return jsonData;
    //}
    public bool SaveCurrentPlayingLevelId(int newCurrentLevelId)
    {
        try
        {
            int nextLevel = newCurrentLevelId + 1;
            if(nextLevel > allLevelsDataCache.levels.Count)
            {
                nextLevel = newCurrentLevelId;
            }
            currentLevel = nextLevel;
            bool isSave = LoadAndSaveGame.Instance.SaveData(nextLevel.ToString(), CONST.LEVEL_CURRENT);
            if (isSave) { 
                return true;
            }
            return false;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving level ID: {ex.Message}");
            return false;
        }

    }
    //public bool SaveCurrentPlayingLevelId(int newCurrentLevelId)
    //{
    //    try
    //    {
    //        int nextLevel = newCurrentLevelId + 1;
    //        if (nextLevel > allLevelsDataCache.levels.Count)
    //        {
    //            nextLevel = newCurrentLevelId;
    //        }
    //        currentLevel = nextLevel;
    //        string folderPath = Path.Combine(Application.dataPath, CONST.RESOURCES);
    //        string filePath = Path.Combine(folderPath, CONST.LEVEL_CURRENT);

    //        if (!Directory.Exists(folderPath))
    //        {
    //            Directory.CreateDirectory(folderPath);
    //        }

    //        File.WriteAllText(filePath, nextLevel.ToString());

    //        Debug.Log($"Level ID {nextLevel} saved to: {filePath}");
    //        return true;
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.LogError($"Error saving level ID: {ex.Message}");
    //        return false;
    //    }

    //}
    public int CheckLevel(int levelFile)
    {
        if (currentLevel > levelFile)
        {
            return currentLevel;
        }
        return levelFile;
    }

}
