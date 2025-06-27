using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{

    [SerializeField]
    private GameObject uiButton;

    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button shopButton;
    [Header("UI Level")]

    public TextMeshProUGUI level;

    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();
        SetPositionUI();
        SetPress();
        UpdateLevelDisply();


    }

    public override void Show(object data)
    {
        base.Show(data);
        SetPositionUI();
        SetPress();
        UpdateLevelDisply();
    }

    void SetPress()
    {
        playButton.onClick.AddListener(ShowGame);
        shopButton.onClick.AddListener(ShowShop);
    }

    void ShowGame()
    {
        UIManager.Instance.ShowScreen<GamePlay>();
    }
    void ShowShop()
    {
        UIManager.Instance.ShowPopup<PopupShopPannel>();
    }

    void SetPositionUI()
    {
        if (uiButton != null )
        {
            RectTransform rectTransform = GameManager.Instance.mainUICanvas.GetComponent<RectTransform>();
            float canvasHeight = rectTransform.rect.height;
            Vector3 newPosition = uiButton.transform.localPosition;
            if (canvasHeight <= 1921)
            {
                newPosition.y = -750f;
            }
            else
            {
                newPosition.y = -850f;
            }
            uiButton.transform.localPosition = newPosition;

        }
    }
    public void UpdateLevelDisply()
    {
        int level = LevelDataLoader.Instance.GetCurrentLevel();
        int levelTarget = LevelDataLoader.Instance.CheckLevel(level);
        this.level.text = "Level " + levelTarget.ToString();
    }
}
