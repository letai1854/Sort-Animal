using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : BasePopup
{
    [SerializeField]
    GameObject Baron;
    [SerializeField]
    GameObject Coin;
    [SerializeField]
    Button continueButton;
    [SerializeField]
    Button homeButton;

    public event Action OnHomeButtonPressedEvent;
    public event Action OnButtonContinueEvent;

    private void Awake()
    {
        if (continueButton != null) {
            continueButton.onClick.AddListener(OnlickContinue);
        }
        if (homeButton != null)
        {
            homeButton.onClick.AddListener(HandleHomeClick);
        }
    }
    public override void Hide()
    {
        base.Hide();
        SetUpContentPopup();
    }

    public override void Init()
    {
        base.Init();
        SetUpContentPopup();
    }
    void SetUpContentPopup()
    {
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutSine);
        Baron.gameObject.SetActive(false);
        Coin.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
        Baron.gameObject.transform.localScale = Vector3.zero;
        Coin.gameObject.transform.localScale = Vector3.zero;
        continueButton.gameObject.transform.localScale = Vector3.zero;
        homeButton.gameObject.transform.localScale = Vector3.zero;
    }

    public override void Show(object data)
    {
        base.Show(data);
        Baron.gameObject.SetActive(true);
        Coin.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        StartCoroutine(ShowPopupWinCoroutin());


    }
    private IEnumerator ShowPopupWinCoroutin()
    {
        yield return new WaitForSeconds(0.2f);
        Baron.gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutExpo);
        yield return new WaitForSeconds(0.2f);
        continueButton.gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutExpo);
        yield return new WaitForSeconds(0.2f);
        homeButton.gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutExpo);
        yield return new WaitForSeconds(0.2f);

        //continueButton.gameObject.transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.4f).SetEase(Ease.InOutExpo);
        //yield return new WaitForSeconds(0.3f);
        //continueButton.gameObject.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutSine);

        //homeButton.gameObject.transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.3f).SetEase(Ease.InOutExpo);
        //yield return new WaitForSeconds(0.2f);
        //homeButton.gameObject.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InOutSine);

        Coin.gameObject.transform.DOScale(new Vector3 (1.45f,1.45f,1.45f), 0.3f).SetEase(Ease.InOutExpo);
        yield return new WaitForSeconds(0.3f);
        Coin.gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutSine);
        //EffectManager.Instance.RandomEffectFireWorkWin();
        EffectManager.Instance.StartRandomFireworkWinLoop();


    }

    private void OnlickContinue()
    {
        UIManager.Instance.HideAllPopups();
        OnButtonContinueEvent.Invoke();
        EffectManager.Instance.StopRandomFireworkWinLoop();
    }
    private void HandleHomeClick()
    {
        UIManager.Instance.HideAllPopups();
        EffectManager.Instance.StopRandomFireworkWinLoop();
        UIManager.Instance.ShowScreen<HomeScreen>();
    }
}
