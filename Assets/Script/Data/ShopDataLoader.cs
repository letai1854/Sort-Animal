using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataChoiceItem
{
    public Sprite SpriteBG;
    public AnimalHabitatType habitatType;
}
public class ShopDataLoader 
{

     public List<DataItem> allHabitatAnimals = new List<DataItem>();
     public List<DataItem> allShopBGs = new List<DataItem>();
    GameSaveData currentSaveData = new GameSaveData
    {
        habitatAnimalStates = new List<ItemState>(),
        shopBGStates = new List<ItemState>()
    };
    public ShopDataLoader(List<DataItem> habitatAnimalStates, List<DataItem> shopBGStates)
    {
        this.allHabitatAnimals = habitatAnimalStates;
        this.allShopBGs = shopBGStates;
    }
    public int LoadCoin()
    {
        string json = LoadAndSaveGame.Instance.ReadData(CONST.SAVE_FILE_NAME);
        currentSaveData = JsonConvert.DeserializeObject<GameSaveData>(json);
        int coins = currentSaveData.playerCoins;
        return coins;
    }
    public void SaveGame(List<DataItem> allHabitatAnimals, List<DataItem> allShopBGs, int price = 0)
    {
        var saveData = new GameSaveData();
        saveData.playerCoins = price;
        foreach (var item in allHabitatAnimals)
        {
            saveData.habitatAnimalStates.Add(new ItemState
            {
                itemName = item.itemName,
                isPurchased = item.isPurchasable,
                isSelected = item.isChoiceItem
            });
        }

        foreach (var item in allShopBGs)
        {
            saveData.shopBGStates.Add(new ItemState
            {
                itemName = item.itemName,
                isPurchased = item.isPurchasable,
                isSelected = item.isChoiceItem
            });
        }

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        LoadAndSaveGame.Instance.WriteData(json, CONST.SAVE_FILE_NAME);
        Debug.Log("Game Saved!");
    }
    public DataChoiceItem LoadShopItem()
    {
        string json = LoadAndSaveGame.Instance.ReadData(CONST.SAVE_FILE_NAME);
        
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log("No save file found. Creating a new game.");
        }
        else
        {
            Debug.Log("Save file found. Loading data.");
            currentSaveData = JsonConvert.DeserializeObject<GameSaveData>(json);
        }

       DataChoiceItem dataChoiceItem =  ApplyLoadedData();
        if (dataChoiceItem != null) {
            return dataChoiceItem;
        }
        return null;
    }

    private DataChoiceItem ApplyLoadedData()
    {
        DataChoiceItem dataChoiceItem = new DataChoiceItem();

        foreach (var itemState in currentSaveData.habitatAnimalStates)
        {
            DataItem originalItem = allHabitatAnimals.FirstOrDefault(d => d.itemName == itemState.itemName);
            if (originalItem != null)
            {
                originalItem.isPurchasable = itemState.isPurchased;
                originalItem.isChoiceItem = itemState.isSelected;
                if( originalItem.isChoiceItem )
                {
                    dataChoiceItem.habitatType = originalItem.habitatType;
                }
            }
        }

        foreach (var itemState in currentSaveData.shopBGStates)
        {
            DataItem originalItem = allShopBGs.FirstOrDefault(d => d.itemName == itemState.itemName);
            if (originalItem != null)
            {
                originalItem.isPurchasable = itemState.isPurchased;
                originalItem.isChoiceItem = itemState.isSelected;
                if (originalItem.isChoiceItem)
                {
                    dataChoiceItem.SpriteBG = originalItem.icon;

                }
            }
        }
        return dataChoiceItem;
    }
    public void SaveCoin(int newCoinValue)
    {
        string json = LoadAndSaveGame.Instance.ReadData(CONST.SAVE_FILE_NAME);

        GameSaveData saveData;

        if (string.IsNullOrEmpty(json))
        {
          
            saveData = new GameSaveData();
        }
        else
        {
      
            saveData = JsonConvert.DeserializeObject<GameSaveData>(json);
        }

        saveData.playerCoins = newCoinValue;

        string updatedJson = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        LoadAndSaveGame.Instance.WriteData(updatedJson, CONST.SAVE_FILE_NAME);

        Debug.Log($"Coins saved. New value: {newCoinValue}");
    }


}
