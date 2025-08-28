using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupShopPannel : BasePopup
{
    [SerializeField] private Button buttonBG;
    [SerializeField] private Button buttonAnimal;
    [SerializeField] public RectTransform content;
    [SerializeField] private Button back;

    [SerializeField] public Sprite imageTabChoice;

    [SerializeField] public Image imageButtonAnimal;

    [SerializeField] public Image imageButtonBG;

    public  event Action<TypeItem> OnClickButtonAnimalEvent;
    public  event Action<TypeItem> OnClickButtonBGEvent;

    public event Action<TypeItem> InitShopItem;

     public Sprite imageButtonAnimalBackup;

     public Sprite imageButtonBGBackup;

    public TextMeshProUGUI textCoin;

    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();
        InitShopItem?.Invoke(TypeItem.Background);
        SetUpContentPopup();
        BackHome();
        OnClickButtonAnimal();
        OnClickButtonBG();
        LoadCoinPopup();
    }

    public override void Show(object data)
    {
        base.Show(data);
        SetUpContentPopup();
        OnClickButtonAnimal();
        OnClickButtonBG();
        LoadCoinPopup();
    }

    public void LoadCoinPopup()
    {
        int coins = GameManager.Instance.shopDataLoader.LoadCoin();
        if (coins > 100000)
        {
            textCoin.text = "99999";
        }
        else
        {
            textCoin.text = coins.ToString();

        }
    }
    public void OnClickButtonAnimal()
    {
        buttonAnimal.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClick();

            OnClickButtonAnimalEvent?.Invoke(TypeItem.Animal);
        });
    }
    public void OnClickButtonBG()
    {
        buttonBG.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClick();

            OnClickButtonBGEvent?.Invoke(TypeItem.Background);
        });
    }

    void BackHome()
    {

        back.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClick();
            if(UIManager.Instance.CurOverlap != null)
                UIManager.Instance.CurOverlap.Hide();

            this.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                Hide();
            });
        });
    }
    public void SetImageTabChoice(TypeItem typeItem)
    {
        if(typeItem == TypeItem.Background)
        {
            imageButtonBG.sprite = imageTabChoice;
            imageButtonAnimal.sprite = imageButtonAnimalBackup;
        }
        else if(typeItem == TypeItem.Animal)
        {
            imageButtonAnimal.sprite = imageTabChoice;
            imageButtonBG.sprite = imageButtonBGBackup;
        }
    }
    void SetUpContentPopup()
    {
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutSine);
    }
    public void SetUpContent(int countItem)
    {
        int itemsPerRow = 2;
        int rowCount = Mathf.CeilToInt((float)countItem / itemsPerRow);
        int height = rowCount * CONST.SPACE_CONTENT;

        content.sizeDelta = new Vector2(content.sizeDelta.x, height);
    }
}
