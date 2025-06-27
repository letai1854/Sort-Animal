using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.U2D;
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
    public enum TapCurrent
    {
        Background,
        Animal,
    }
    private void Awake()
    {
       
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
            GameObject itemShop = Instantiate(shopItemPrefab, content);
            ShopItemView shopItemView = itemShop.GetComponent<ShopItemView>();
            shopItemView.SetUpItem(data.icon, data.itemName, data.value, i, data.isPurchasable, data.isChoiceItem);
            shopItemViewsList.Add(shopItemView);
            dataItemList.Add(data);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
