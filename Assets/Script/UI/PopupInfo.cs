using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static ShopContronler;

public class PopupInfo : BasePopup
{
    [SerializeField]
    public Button cancleButton;

    public override void Hide()
    {
        this.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            base.Hide();
        });
    }

    public override void Init()
    {
        base.Init();
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
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutSine);



    }
}
