using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopContronler;

public class PopupBuy : BaseOverlap
{
    [SerializeField]
    TextMeshProUGUI nameItem;
    [SerializeField]
    TextMeshProUGUI priceItem;

    [SerializeField]
    public Button nobuyButton;
    [SerializeField]
    public Button buyButton;
    private event Action<int, ChoseItem> onBuyEvent;
    private int idItem;
    public override void Hide()
    {
        this.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            base.Hide();
        });
    }

    public override void Init()
    {
        base.Init();
        buyButton.onClick.AddListener(OnClickBuy);
        nobuyButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonClick();

            Hide();     
        });
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutSine);
    }

    public override void Show(object data)
    {
        base.Show(data);
        DataItemView dataItemView = data as DataItemView;
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutSine);
        nameItem.text = dataItemView.itemName;
        priceItem.text = dataItemView.value.ToString();
        onBuyEvent = dataItemView.onclickItemEvent;
        idItem = dataItemView.id;
    }
    public void OnClickBuy()
    {
        SoundManager.Instance.PlayButtonClick();
        onBuyEvent?.Invoke(idItem, ChoseItem.Buy);
        Hide();
    }
}
