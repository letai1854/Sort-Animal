using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardView : MonoBehaviour
{
    [Header("View References")]

    [SerializeField]
    private Image backgroundImage;

    [Header("UI Button")]

    public Button undoButton;
    public Button restartButton;
    public Button homeButton;

    public TextMeshProUGUI textCountUndo;
    public event Action OnUndoButtonPressedEvent;
    public event Action OnRestartButtonPressedEvent;
    public event Action OnHomeButtonPressedEvent;


    [Header("UI Level")]

    public TextMeshProUGUI level;


    //public RectTransform tubesContainerTransform;
    private void Awake()
    {
        if (undoButton!=null)
        {
            undoButton.onClick.AddListener(HandleUndoClick);
        }
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(HandleRestartClick);
        }
        if (homeButton != null)
        {
            homeButton.onClick.AddListener(HandleHomeClick);
        }
        UpdateUndoCountDisplay(CONST.MAX_MOVE_HISTORY);
    }


    public void SetupAllTubeViews(int tubeHeight, GridLayoutGroup tubesGridLayout, RectTransform tubesContainerTransform)
    {

        tubesGridLayout.cellSize = new Vector2(tubesGridLayout.cellSize.x, TubeViewAssetManager.Instance.CellSize(tubeHeight));
        RectTransform rectTransform = GameManager.Instance.mainUICanvas.GetComponent<RectTransform>();
        float canvasHeight  = rectTransform.rect.height;
        if (canvasHeight > 2300)
        {
            Vector2 newSize = tubesContainerTransform.sizeDelta;
            newSize.y = canvasHeight;
            tubesContainerTransform.sizeDelta = newSize;
            tubesGridLayout.padding.top = 600;
        }
        if (canvasHeight <= 1921)
        {
            tubesGridLayout.transform.DOScale(new Vector3(0.9f,0.9f,1f),0f);
            if (rectTransform.rect.width > 1300 && rectTransform.rect.width <= 1441)
            {
                Vector2 spacing = tubesGridLayout.spacing;
                spacing.x = 120;
                tubesGridLayout.spacing = spacing;
            }
        }
        else if (canvasHeight >= 2159)
        {
            Vector2 newSize = tubesContainerTransform.sizeDelta;
            newSize.y = canvasHeight;
            tubesContainerTransform.sizeDelta = newSize;
            tubesGridLayout.padding.top = 450;

        }
    
    }
    private void HandleUndoClick()
    {
        OnUndoButtonPressedEvent?.Invoke(); 
    }

    private void HandleRestartClick()
    {
        OnRestartButtonPressedEvent?.Invoke();
    }
    private void HandleHomeClick()
    {
        UIManager.Instance.ShowScreen<HomeScreen>();
    }
    public void UpdateUndoCountDisplay(int remainingUndos)
    {
        if (textCountUndo != null)
        {
            textCountUndo.text = remainingUndos.ToString();
        }
    }
    public void UpdateLevelDisply(int level)
    {
        this.level.text = "Level "+level.ToString();
    }
   public void SetBackground(Sprite backgroundSprite)
    {
        if (backgroundImage != null && backgroundSprite != null)
        {
            backgroundImage.sprite = backgroundSprite;
        }
    }

}



