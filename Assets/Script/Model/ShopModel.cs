using System.Collections.Generic;
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
    public void UpdateItem(DataItem newData ,DataItem dataItem )
    {
        
    }
}
