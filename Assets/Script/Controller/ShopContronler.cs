using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public enum TypeItem
{
    Background,
    Animal,
}

public class ShopContronler : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] GameObject shopItemPrefab;
    ShopModel shopModel;
    public List<DataItem> dataItemList;
    public List<ShopItemView> shopItemViewsList;

    List<DataItem> habitatAnimal;
    List<DataItem> shopBGDataList;

    List<DataItem> currentListData;

    TypeItem tapCurrent = TypeItem.Background;
    BasePopup popupCur;
    PopupShopPannel popupShopPannel;
    int countBG;
    int idChoice;
    public enum TapCurrent
    {
        Background,
        Animal,
    }

    public enum ChoseItem
    {
        Choice,
        NOChoice,
        Buy,
    }

    event Action <int,ChoseItem> OnClickItemEvent;

    private void Awake()
    {
       OnClickItemEvent += OnBuyOrChoiceItem;
    }
    
    void Start()
    {
        dataItemList = new List<DataItem>();
        shopItemViewsList = new List<ShopItemView>();

        shopModel = new ShopModel(GameManager.Instance.habitatAnimal,GameManager.Instance.shopBGDataList);
        

        popupCur = UIManager.Instance.CurPopup;
        popupShopPannel = popupCur.GetComponent<PopupShopPannel>();
        popupShopPannel.OnClickButtonAnimalEvent += OnclickTap;
        popupShopPannel.OnClickButtonBGEvent += OnclickTap;

        popupShopPannel.imageButtonAnimalBackup = popupShopPannel.imageButtonAnimal.sprite;
        popupShopPannel.imageButtonBGBackup = popupShopPannel.imageButtonBG.sprite;

        content = popupShopPannel.content;
        //popupCur.GetComponent<PopupShopPannel>().InitShopItem += SetUPAllItem;

        habitatAnimal = shopModel.habitatAnimal;
        shopBGDataList = shopModel.shopBGDataList;
        SetUPAllItem(TypeItem.Background);
    }


    private void OnclickTap(TypeItem type)
    {
        if(type != tapCurrent)
        {
            
            SetUPAllItem(type);
        }
       
    }
    public void SetUPAllItem(TypeItem typeItem)
    {
        tapCurrent = typeItem;
        countBG = GameManager.Instance.shopBGDataList.Count;
        popupShopPannel.SetUpContent(countBG);
        popupShopPannel.SetImageTabChoice(typeItem);
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
            shopItemViewsList.Clear();
            dataItemList.Clear();
        }
        if (typeItem == TypeItem.Background)
        {
            currentListData = shopBGDataList;

        }
        else 
        {
            currentListData = habitatAnimal;
        }

        SetUpItem(currentListData);
    }
    private void SetUpItem(List<DataItem> shopDataList)
    {
        for (int i = 0; i < shopDataList.Count; i++)
        {
            DataItem data = shopDataList[i];
            if (data.isChoiceItem)
            {
                idChoice = i;
            }
            GameObject itemShop = Instantiate(shopItemPrefab, content);
            ShopItemView shopItemView = itemShop.GetComponent<ShopItemView>();
            shopItemView.SetUpItem(data.icon, data.itemName, data.value, i, data.isPurchasable, data.isChoiceItem, OnClickItemEvent);
            shopItemViewsList.Add(shopItemView);
            dataItemList.Add(data);
        }
    }
    void Update()
    {

    }
    void OnBuyOrChoiceItem(int id, ChoseItem isChoice)
    {
        int price = GameManager.Instance.shopDataLoader.LoadCoin();
        switch (isChoice)
        {
            case ChoseItem.Choice:
                Debug.Log("Choice item: " + id);
                bool isChoiceItem = shopModel.GetItemChoice(dataItemList[id], dataItemList[idChoice]);
                if (isChoiceItem)
                    shopItemViewsList[idChoice].isChoiceItem = false;
                    shopItemViewsList[idChoice].UpdateVisuals();
                    shopItemViewsList[id].isChoiceItem = true;
                    shopItemViewsList[id].UpdateVisuals();
                    idChoice = id;
                    GameManager.Instance.shopDataLoader.SaveGame(habitatAnimal, shopBGDataList, price);
                    GameManager.Instance.dataChoiceItem = GameManager.Instance.shopDataLoader.LoadShopItem();
                break;
            case ChoseItem.Buy:
                bool isBuy = shopModel.CheckItemPurchased(dataItemList[id], price);
                if (isBuy)
                {
                    shopItemViewsList[id].isPurchasable = true;
                    shopItemViewsList[id].UpdateVisuals();
                    int priceTotal = price - dataItemList[id].value;
                    GameManager.Instance.shopDataLoader.SaveGame(habitatAnimal, shopBGDataList, priceTotal);
                    BasePopup basePopup = UIManager.Instance.CurPopup;
                    basePopup.GetComponent<PopupShopPannel>().LoadCoinPopup();
                }
                break;
        }
  
    }

}
