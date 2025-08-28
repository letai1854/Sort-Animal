using System.Collections.Generic;
using UnityEngine;





public class GameManager : SingletonMonoBehaviour<GameManager>
{


    public Canvas mainUICanvas;
    public PlayerSettings playerSettings;
    private PlayerDataService playerDataService;


    public List<DataItem> habitatAnimal;
    public List<DataItem> shopBGDataList;


    public ShopDataLoader shopDataLoader;
    public DataChoiceItem dataChoiceItem;
    private void Awake()
    {
        shopDataLoader = new ShopDataLoader(habitatAnimal, shopBGDataList);
        playerDataService = new PlayerDataService();
        playerSettings = playerDataService.LoadPlayerSettings(CONST.SETTINGLOAD);

    }
    void Start()
    {
        UIManager.Instance.ShowScreen<HomeScreen>();

        string saveFilePath = System.IO.Path.Combine(Application.persistentDataPath, CONST.SAVE_FILE_NAME);
        if (!System.IO.File.Exists(saveFilePath))
        {
            Debug.Log("Save file not found. Creating initial game data for the first launch.");

            int startingCoins = 9999;
            shopDataLoader.SaveGame(habitatAnimal, shopBGDataList, startingCoins);
        }
        else
        {

            Debug.Log("Existing save file found. Skipping initial save.");
        }

        dataChoiceItem =  shopDataLoader.LoadShopItem();
    }

    void Update()
    {
        
    }
}
