using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDataItem", menuName = "Shop/Data Item")]
public class DataItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int value;
    public bool isPurchasable;
    public bool isChoiceItem;
    public AnimalHabitatType habitatType;
}



[System.Serializable]
public class ItemState
{
    public string itemName;
    public bool isPurchased;
    public bool isSelected; 
}

[System.Serializable]
public class GameSaveData
{
    public int playerCoins;
    public List<ItemState> habitatAnimalStates = new List<ItemState>();
    public List<ItemState> shopBGStates = new List<ItemState>();
}

public class CoinData
{
    public int coins;
    public int totalCoins; 
}
