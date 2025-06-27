using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
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
    void Start()
    {
        //tickImage.gameObject.SetActive(false);
        //coins.SetActive(false);
        //imageItem.color = new Color32(128, 128, 128, 255);
    }
    public void SetUpItem(Sprite sprite, string name, int price, int id, bool isPurchasable, bool isChoiceItem)
    {
        imageItem.sprite = sprite;
        textName = name;
        this.price = price;
        this.id = id;
        this.isPurchasable = isPurchasable;
        this.isChoiceItem = isChoiceItem;

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

}
