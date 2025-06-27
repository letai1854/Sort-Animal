using DG.Tweening;
using System;
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
    }

    public override void Show(object data)
    {
        base.Show(data);
        BackHome();
        SetUpContentPopup();
        OnClickButtonAnimal();
        OnClickButtonBG();
    }

    public void OnClickButtonAnimal()
    {
        buttonAnimal.onClick.AddListener(() =>
        {
            OnClickButtonAnimalEvent?.Invoke(TypeItem.Animal);
        });
    }
    public void OnClickButtonBG()
    {
        buttonBG.onClick.AddListener(() =>
        {
            OnClickButtonBGEvent?.Invoke(TypeItem.Background);
        });
    }

    void BackHome()
    {
        back.onClick.AddListener(() =>
        {
            UIManager.Instance.HideAllPopups();
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
