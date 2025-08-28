using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopContronler;

public class PopupUseOrNotUse : BaseOverlap
{
    [SerializeField]
    TextMeshProUGUI nameItem;

    [SerializeField]
    TextMeshProUGUI nameButton;

    [SerializeField]
    public Button choiceItem;
    [SerializeField]
    public Button cancleButton;
    private event Action<int, ChoseItem> onChoiceItem;
    private int idItem;
    private bool isChoice;
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
        choiceItem.onClick.AddListener(OnClickUse);
        cancleButton.onClick.AddListener(() =>
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
        onChoiceItem = dataItemView.onclickItemEvent;
        idItem = dataItemView.id;
        isChoice = dataItemView.isChoice;

           nameButton.text = "Use";
        
    }
    public void OnClickUse()
    {
        SoundManager.Instance.PlayButtonClick();

        onChoiceItem?.Invoke(idItem, ChoseItem.Choice);
        Hide();
    }
}
