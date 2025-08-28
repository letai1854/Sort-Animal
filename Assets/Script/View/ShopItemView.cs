using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ShopContronler;

    public class DataItemView
    {
        public string itemName;
        public int value;
        public int id;
        public Action<int, ChoseItem> onclickItemEvent;
        public bool isChoice;
}
public class ShopItemView : MonoBehaviour, IPointerClickHandler
{
    public Image imageItem;
    public Image tickImage;
    public GameObject coins;
    public string textName;
    public int price;
    public int id;
    public bool isPurchasable;
    public bool isChoiceItem;
    public TextMeshProUGUI priceText;


    DataItemView dataItemView;
    event Action<int> onclickItemEvent;
    void Start()
    {
        //tickImage.gameObject.SetActive(false);
        //coins.SetActive(false);
        //imageItem.color = new Color32(128, 128, 128, 255);
    }
    public void SetUpItem(Sprite sprite, string name, int price, int id, bool isPurchasable, bool isChoiceItem, Action<int, ChoseItem> onclickItem)
    {
 
        imageItem.sprite = sprite;
        textName = name;
        this.price = price;
        this.id = id;
        this.isPurchasable = isPurchasable;
        this.isChoiceItem = isChoiceItem;

        dataItemView = new DataItemView
        {
            itemName = name,
            value = price,
            id = id,
            onclickItemEvent = onclickItem,
            isChoice = isChoiceItem
        };
        //SetPurchasable(isPurchasable);
        //SetTickImage(isChoiceItem);
        UpdateVisuals();

    }
    void Update()
    {

    }

    public void UpdateVisuals()
    {
        if (isChoiceItem)
        {
            imageItem.color = Color.white;
            coins.SetActive(false);
            tickImage.gameObject.SetActive(true);
        }
        else if (isPurchasable)
        {
            imageItem.color = Color.white;
            coins.SetActive(false);
            tickImage.gameObject.SetActive(false);
        }
        else
        {
            imageItem.color = new Color32(128, 128, 128, 255); 
            coins.SetActive(true);
            priceText.text = price.ToString();
            tickImage.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlayButtonClick();

        if (!isPurchasable)
        {

            UIManager.Instance.ShowOverlap<PopupBuy>(dataItemView);
            return;
        }    
        if (!isChoiceItem)
        {

                UIManager.Instance.ShowOverlap<PopupUseOrNotUse>(dataItemView);
         
        }
    }
}
