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

    public int CheckLevel(int levelFile)
    {
        if (currentLevel > levelFile)
        {
            return currentLevel;
        }
        return levelFile;
    }

    public int GetCurrentLevel()
    {
        string levelString = LoadAndSaveGame.Instance.ReadData(CONST.LEVEL_CURRENT_PATH);

        if (string.IsNullOrEmpty(levelString) || !int.TryParse(levelString, out int savedLevel))
        {
            return 1;
        }

        return savedLevel;

    }
    public bool SaveCurrentPlayingLevelId(int newCurrentLevelId)
    {
        try
        {
            if (allLevelsDataCache == null || allLevelsDataCache.levels == null || allLevelsDataCache.levels.Count == 0)
            {
                Debug.LogError("All levels data cache is not loaded. Cannot determine next level.");
                return false;
            }

            int nextLevel = newCurrentLevelId + 1;

            if (nextLevel > allLevelsDataCache.levels.Count)
            {
                nextLevel = allLevelsDataCache.levels.Count;
                Debug.Log("Player has completed all levels. Setting to the last level.");
            }

            currentLevel = nextLevel;

            bool isSaved = LoadAndSaveGame.Instance.WriteData(nextLevel.ToString(), CONST.LEVEL_CURRENT_PATH);

            return isSaved;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving level ID: {ex.Message}");
            return false;
        }

    }





   
}
