using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameModle;



public class GameController : MonoBehaviour
{
    private enum SelectionState
    {
        SelectingSource,
        SourceSelected,
        AnimatingMove,
        Complete
    }



    [Header("Game Model Data")]
    private GameModle gameModel;
    [Header("View References")]
    
    public GridLayoutGroup tubesGridLayout;
    public RectTransform tubesContainerTransform;
    private List<TubeView> tubeViewInstances = new List<TubeView>();


    [Header("Select tube")]
    private SelectionState currentSelectionState = SelectionState.SelectingSource;
    private int indexSource = -1;
    private int indexTarget = -1;

    [SerializeField]
    GameBoardView boardView;
    MoveOperation canMove;
    public static  event Action animalMoving;


    [Header("Hint Settings")]
    public float timeUntilHintAppears = 10.0f; 

    private float noActionTimer = 0f;
    [Header("Undo Logic & State")]
    public  int maxUndoPerLevel;
    int hintId = 0;
    protected  void Awake()
    {
        TubeView.OnTubeTapped += HandleTubeTapEvent;
        animalMoving += ResetSelectionAndShowEffect;
        boardView.OnUndoButtonPressedEvent += HandleUndo;
        boardView.OnRestartButtonPressedEvent += ReStartGame;

      
      
    }
    private void OnDestroy()
    {
        TubeView.OnTubeTapped -= HandleTubeTapEvent;
        boardView.OnUndoButtonPressedEvent -= HandleUndo;
        animalMoving -= ResetSelectionAndShowEffect;

    }
    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if(currentSelectionState == SelectionState.SelectingSource )
        {
            noActionTimer += Time.deltaTime;
            if(noActionTimer > timeUntilHintAppears) {

                TriggerAutoHint();
            }
        }
    }
    private void ReStartGame()
    {
        //if(MoveHistoryManager.Instance.CanUndo())
        //{
        //}
            StartGame();
    }
    public void StartGame()
    {
        //gameModel = new GameModle(tubeCount, tubeHeight, tubeEmpty);
        indexSource = -1;
        indexTarget = -1;
        currentSelectionState = SelectionState.SelectingSource;
        noActionTimer = 0;
        gameModel = new GameModle();
        gameModel.NewGame();
        boardView.UpdateLevelDisply(gameModel.level);
        maxUndoPerLevel = CONST.MAX_MOVE_HISTORY;
        boardView.UpdateUndoCountDisplay(maxUndoPerLevel);
        MoveHistoryManager.Instance.ClearHistory();
        SetupAllTubeViews(gameModel.tubeHeight);
    }
    public void SetupAllTubeViews(int tubeHeight)
    {
        if(tubesContainerTransform == null)
        {
            GameObject tubeContainerObj = GameObject.Find(CONST.TUBELAYOUT);
            if (tubeContainerObj != null)
            {
                tubesContainerTransform = tubeContainerObj.GetComponent<RectTransform>();
                Debug.Log("Found tubesContainerTransform in the scene by name");
            }

        }
        boardView.SetBackground(GameManager.Instance.dataChoiceItem.SpriteBG);
        foreach (Transform child in tubesContainerTransform) 
        {
            Destroy(child.gameObject);
        }
        tubeViewInstances.Clear();
        for (int i = 0; i < gameModel.tubes.Count; i++) 
        {
            TubeModle currentTubeModel = gameModel.tubes[i];
            boardView.SetupAllTubeViews(tubeHeight, tubesGridLayout, tubesContainerTransform);
            GameObject tubePrefabs = TubeViewAssetManager.Instance.GetPrefabsForCapacity(tubeHeight);

            if (tubePrefabs != null)
            {
                GameObject tubeObjView = Instantiate( tubePrefabs, tubesContainerTransform);
                TubeView tubeView = tubeObjView.GetComponent<TubeView>();
                tubeView.Initialize(currentTubeModel, i,tubeHeight,GameManager.Instance.dataChoiceItem.habitatType);
                tubeViewInstances.Add(tubeView);
                tubeView.ShowView();
            }
        }

    }
    private void HandleTubeTapEvent(int index)
    {
        if(index <0 || index >= gameModel.tubes.Count) return;
        noActionTimer = 0f;
        switch (currentSelectionState)
        {
            case SelectionState.SelectingSource:
                ChoseTubeSource(index);
                break;

            case SelectionState.SourceSelected:
                EffectManager.Instance.StopArrowEffect();
                
                indexTarget = index;
                canMove =  gameModel.MoveAnimal(indexSource,  indexTarget);
                if (canMove is not null )
                {
                    MoveOperation operation = new MoveOperation(canMove.SourceTubeIndex,canMove.DestinationTubeIndex,canMove.MovedAnimals);
                    MoveHistoryManager.Instance.RecordMove(operation);
                    currentSelectionState = SelectionState.AnimatingMove;
                    tubeViewInstances[canMove.SourceTubeIndex].MoveTubeTarget(tubeViewInstances[canMove.DestinationTubeIndex],canMove.MovedAnimals,animalMoving);
                    tubeViewInstances[indexSource].SetSelectedHighlight(false);
                    if (currentSelectionState != SelectionState.AnimatingMove)

                        currentSelectionState = SelectionState.SelectingSource;
                        tubeViewInstances[indexSource].SetSelectedHighlight(false);
                        ResetStateSelection();
                    Debug.Log(operation);
                }
                else
                {
                    bool checkPrevent = false;
                    if (indexSource == indexTarget)
                    {
                        currentSelectionState = SelectionState.AnimatingMove;
                        checkPrevent = true;
                    }
                    tubeViewInstances[indexSource].SetSelectedHighlight(false,animalMoving, checkPrevent);
                    if(indexSource != indexTarget)
                    {
                        indexSource = indexTarget;
                        currentSelectionState = SelectionState.SourceSelected;
                        tubeViewInstances[indexTarget].SetSelectedHighlight(true);
                    }
                    else
                    {
                        int tubeAnimalCount = gameModel.tubes[indexSource].animals.Count;
                        if (tubeAnimalCount == 0)
                        {
                            currentSelectionState = SelectionState.SelectingSource;
                        }
                        ResetStateSelection();
                    }
                }


                break;
              
        }
        isCompleteGame();


    }
    void ChoseTubeSource(int index)
    {
        indexSource = index;
        currentSelectionState = SelectionState.SourceSelected;
        tubeViewInstances[indexSource].SetSelectedHighlight(true);
    }
    private void ResetStateSelection()
    {
        //currentSelectionState = SelectionState.SelectingSource;
        //tubeViewInstances[indexSource].SetSelectedHighlight(false);
        indexSource = -1;
        indexTarget = -1;
    }
    private void ResetSelectionAndShowEffect()
    {
        currentSelectionState = SelectionState.SelectingSource;
        ShowEffect();


    }
    private void ShowEffect()
    {
        if (tubeViewInstances[canMove.DestinationTubeIndex].CheckEffectSpawnPosition())
        {
          Vector3 vector3 =   tubeViewInstances[canMove.DestinationTubeIndex].GetEffectSpawnPosition();
          EffectManager.Instance.PlayTubeCompleteEffectAtWorldPosition(vector3);
            tubeViewInstances[canMove.DestinationTubeIndex].GetEffectAnimals();
        }
        //tubeViewInstances[canMove.DestinationTubeIndex].ShowEffect();
    }
    private void TriggerAutoHint()
    {
        noActionTimer = 0f;
        HintMove hint = gameModel.FindNextHint();
        if(hint is not null)
        {
            if (hint.IsValid)
            {
                ChoseTubeSource(hint.SourceTubeIndex);
                hintId = hint.SourceTubeIndex;
                EffectManager.Instance.ArrowEffect(tubeViewInstances[hint.DestinationTubeIndex]);
            }
        }

    }
    public void HandleUndo()
    {
        noActionTimer = 0;
        
        if (maxUndoPerLevel > 0 && currentSelectionState != SelectionState.AnimatingMove)
        {
            if (MoveHistoryManager.Instance.CanUndo())
            {
                EffectManager.Instance.StopArrowEffect();
                tubeViewInstances[hintId].SetSelectedHighlight(false);


                MoveOperation moveOperation = MoveHistoryManager.Instance.GetLastMoveForUndoAndPop();
                gameModel.Undo(moveOperation);
                maxUndoPerLevel--;
                boardView.UpdateUndoCountDisplay(maxUndoPerLevel);
                tubeViewInstances[moveOperation.SourceTubeIndex].ShowView();
                tubeViewInstances[moveOperation.DestinationTubeIndex].ShowView();
                currentSelectionState = SelectionState.SelectingSource;
            }
            else
            {
                EffectManager.Instance.NotifyUndo();
            }
        }

    }
    private void isCompleteGame()
    {
        bool isComplete =  gameModel.IsLevelComplete();
        if (isComplete)
        {
            LevelDataLoader.Instance.SaveCurrentPlayingLevelId(gameModel.level);
            currentSelectionState = SelectionState.Complete;
            noActionTimer = -Mathf.Infinity;
            StartCoroutine(ShowPopupWinCoroutin());
        }    
        Debug.Log(isComplete);
    }
    
    private IEnumerator ShowPopupWinCoroutin()
    {
        yield return new WaitForSeconds(0.6f);

        int coin = CoinWin();
        int price = GameManager.Instance.shopDataLoader.LoadCoin();
        price += coin;
        CoinData coinData = new CoinData();
        coinData.coins = coin;
        coinData.totalCoins = price;
        UIManager.Instance.ShowPopup<PopupWin>(coinData);
        GameManager.Instance.shopDataLoader.SaveCoin(price);
        BasePopup basePopup = UIManager.Instance.CurPopup;
        PopupWin popupWin = basePopup.GetComponent<PopupWin>();
        popupWin.OnButtonContinueEvent += StartGame;

    }

    public int CoinWin()
    {
        int coinWin = UnityEngine.Random.Range(5, 25);
        return coinWin;
    }
}
