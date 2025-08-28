using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManager;


public class ShopModel 
{
    public List<DataItem> habitatAnimal;
    public List<DataItem> shopBGDataList;

    public ShopModel(List<DataItem> habitatAnimal, List<DataItem> shopBGDataList)
    {
        this.habitatAnimal = habitatAnimal;
        this.shopBGDataList = shopBGDataList;
    }
    public bool CheckItemPurchased(DataItem dataItem ,int price)
    {
        DataItem originalItem = habitatAnimal.FirstOrDefault(d => d.itemName == dataItem.itemName);
        if (originalItem != null)
        {
            return CheckPrice(price, originalItem);
        }
        else
        {
             originalItem = shopBGDataList.FirstOrDefault(d => d.itemName == dataItem.itemName);
        }
        return CheckPrice(price, originalItem);
    }

    private  bool CheckPrice(int price, DataItem originalItem)
    {
        if (!originalItem.isPurchasable && originalItem.value <= price)
        {
            originalItem.isPurchasable = true;
            return true;
        }

            return false;
        
    }
    public bool GetItemChoice(DataItem dataItemChoice, DataItem dataItemNotChoice)
    {
        DataItem itemNoChoice = habitatAnimal.FirstOrDefault(d => d.itemName == dataItemNotChoice.itemName);
        if (itemNoChoice != null)
        {
            DataItem itemChoice = habitatAnimal.FirstOrDefault(d => d.itemName == dataItemChoice.itemName);
            itemNoChoice.isChoiceItem = false;
            itemChoice.isChoiceItem = true;
            return true;
        }
        else
        {
            itemNoChoice = shopBGDataList.FirstOrDefault(d => d.itemName == dataItemNotChoice.itemName);
            DataItem itemChoice = shopBGDataList.FirstOrDefault(d => d.itemName == dataItemChoice.itemName);
            itemNoChoice.isChoiceItem = false;
            itemChoice.isChoiceItem = true;
            return true;

        }
        return false;
    }
}
