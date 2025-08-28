using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TubeView : MonoBehaviour, IPointerClickHandler
{
    public TubeModle tubeModle { get; private set; }
    public int tubeIndex { get; private set; }
    [Header("Child Object References")]
    [Tooltip("RectTransform của Panel chứa các AnimalView.")]
    public RectTransform animalsContainerPanelRect;

    [Header("Animal View Setup")]
    [Tooltip("Prefab của một AnimalView")]
    public GameObject animalViewPrefab;
    private List<AnimalView> animalViewInstances = new List<AnimalView>();


    public static event Action<int> OnTubeTapped;

    [Tooltip("Transform của GameObject hiển thị phần nền của ống.")]
    public Transform tubeBackgroundTransform;
    [Tooltip("Transform của GameObject hiển thị phần thủy tinh của ống.")]
    public Transform tubeGlassTransform;



    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    public Vector3 selectedScaleFactor = new Vector3(1.05f, 1.05f, 1f);
    private Vector3 originalScale;

    public float highlightScaleDuration = 0.2f;
    private float waitForBetween = 0.17f;
    public Ease highlightScaleEase = Ease.OutBack;



    [Header("Floating Animal Effect")]
    [Tooltip("Vị trí (local) mà animal trên cùng sẽ bay đến khi ống được chọn làm nguồn.")]
    public Vector3 floatingAnimalPositionOffset = new Vector3(0, 100f, 0);
    [Tooltip("Thời gian cho animation bay lên/xuống của animal.")]
    public float floatingAnimalAnimDuration = 0.1f;
    public Ease floatingAnimalEaseUp = Ease.OutQuad;
    public Ease floatingAnimalEaseDown = Ease.InBack;

    private AnimalView currentlyFloatingAnimalView = null;
    //private Vector3 originalFloatingAnimalLocalPos;
    //private int originalFloatingAnimalSiblingIndex;

    public int tubeHeight;

    bool checkHasWait = false;
    bool checkLastAnimal;
    private AnimalHabitatType animalHabitatType = AnimalHabitatType.Forest;
    void Awake()
    {

        originalScale = transform.localScale;
    }
    public void Initialize(TubeModle tubeModle, int tubeIndex, int tubeHight, AnimalHabitatType animalHabitatType)
    {
        this.tubeModle = tubeModle;
        this.tubeIndex = tubeIndex;
        this.tubeHeight = tubeHight;
        this.animalHabitatType = animalHabitatType;

    }
    public void ShowView()
    {
        if (tubeModle == null)
        {
            Debug.LogError($"TubeView {gameObject.name}: tubeModleDataRef is null in UpdateView.");
            return;
        }
        foreach (var oldAnimal in animalViewInstances)
        {
            if (oldAnimal != null) Destroy(oldAnimal.gameObject);
        }
        animalViewInstances.Clear();
        foreach (AnimalType animalType in tubeModle.animals)
        {
            GameObject animalObj = Instantiate(animalViewPrefab, animalsContainerPanelRect);
            AnimalView animalView = animalObj.GetComponent<AnimalView>();
            animalView.SetAnimal(animalType, animalHabitatType);
            animalViewInstances.Add(animalView);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTubeTapped.Invoke(this.tubeIndex);
    }
    public void SetSelectedHighlight(bool isHighlighted, Action onCompleteCallback = null, bool checkLastAnimal = false)
    {
        transform.DOKill();
        if (animalViewInstances.Count == 0)
        {
            isHighlighted = false;
        }
        if (isHighlighted)
        {
            Vector3 targetScale = Vector3.Scale(originalScale, selectedScaleFactor);



            transform.DOScale(targetScale, highlightScaleDuration)
                     .SetEase(highlightScaleEase);
            MoveAnimalUp();
        }
        else
        {
            transform.DOScale(originalScale, highlightScaleDuration)
                     .SetEase(highlightScaleEase);
            bool movebetween = true;

            if (currentlyFloatingAnimalView != null)
                MoveAnimalDown(onCompleteCallback, animalViewInstances[animalViewInstances.Count - 1], checkHasWait, checkLastAnimal, movebetween);
        }
    }
    public void MoveTubeTarget(TubeView tubeViewTarget, List<AnimalType> animals, Action onCompleteCallback)
    {
        checkLastAnimal = false;
        checkHasWait = true;
        if (currentlyFloatingAnimalView != null)
        {
            AnimalType animalTop = animals[^1];
            if (animalTop == currentlyFloatingAnimalView.animaltype)
            {

                MoveAnimalBetweenTube(animalViewInstances[animalViewInstances.Count - 1], tubeViewTarget);
                if (animals.Count == 1)
                {
                    checkLastAnimal = true;
                }
                tubeViewTarget.MoveAnimalDown(onCompleteCallback, animalViewInstances[animalViewInstances.Count - 1], checkHasWait, checkLastAnimal);

                animals.RemoveAt(animals.Count - 1);
                animalViewInstances.RemoveAt(animalViewInstances.Count - 1);
                currentlyFloatingAnimalView = null;
            }


        }


        MoveAnimals(tubeViewTarget, animals, onCompleteCallback);

    }
    private void MoveAnimals(TubeView tubeViewTarget, List<AnimalType> animals, Action onCompleteCallback)
    {



        StartCoroutine(MoveAnimalsSequence(tubeViewTarget, animals, onCompleteCallback));
    }
    private IEnumerator MoveAnimalsSequence(TubeView tubeViewTarget, List<AnimalType> animals, Action onCompleteCallback)
    {
        for (int i = animals.Count - 1; i >= 0; i--)
        {
            AnimalType animalTop = animals[^1];
            AnimalView animalViewTemp = animalViewInstances[^1];

            if (animalTop == animalViewTemp.animaltype)
            {
                yield return new WaitForSeconds(0.03f);
                MoveAnimalUp();
                yield return new WaitForSeconds(0.2f);
                MoveAnimalBetweenTube(animalViewInstances[animalViewInstances.Count - 1], tubeViewTarget);
                if (i == 0)
                {
                    checkLastAnimal = true;
                }
            

                tubeViewTarget.MoveAnimalDown(onCompleteCallback, animalViewInstances[animalViewInstances.Count - 1], checkHasWait, checkLastAnimal);
                animals.RemoveAt(animals.Count - 1);
                animalViewInstances.RemoveAt(animalViewInstances.Count - 1);
                currentlyFloatingAnimalView = null;
            }
            else
            {
                break;
            }
        }
    }
    public void MoveAnimalUp()
    {
        if (animalViewInstances.Count > 0)
        {
            if (currentlyFloatingAnimalView == null)
            {
                AnimalView topAnimal = animalViewInstances[^1];
                string bounceID = $"bounce_{topAnimal.GetInstanceID()}";
                DOTween.Kill(bounceID);
                currentlyFloatingAnimalView = animalViewInstances[animalViewInstances.Count - 1];


                currentlyFloatingAnimalView.transform.SetParent(this.transform, true);
                currentlyFloatingAnimalView.transform.DOLocalMove(new Vector3(0, 100f, 0), floatingAnimalAnimDuration).SetEase(floatingAnimalEaseUp);
            }
        }
    }

    public void MoveAnimalBetweenTube(AnimalView animalView, TubeView tubeView)
    {
        currentlyFloatingAnimalView = null;
        tubeView.animalViewInstances.Add(animalView);
        animalView.transform.SetParent(tubeView.transform, true);
        animalView.transform.DOLocalMove(new Vector3(0, 100f, 0), floatingAnimalAnimDuration).SetEase(floatingAnimalEaseUp);
    }



    public void MoveAnimalDown(Action onCompleteCallback, AnimalView animalView, bool hasWait = true, bool lastAnimal = false, bool betweenMove = false)
    {
        StartCoroutine(MoveAnimalDownRoutine(onCompleteCallback, animalView, hasWait, lastAnimal, betweenMove));
    }




    private IEnumerator MoveAnimalDownRoutine(Action onCompleteCallback, AnimalView animalView, bool hasWait = true, bool lastAnimal = false, bool betweenMove = false)
    {
        if (hasWait)
            yield return new WaitForSeconds(waitForBetween);
        if (betweenMove)
            currentlyFloatingAnimalView = null;
        AnimalView animalToMoveDown = animalView;


        GameObject placeholder = new GameObject("PositionPlaceholder");
        RectTransform placeholderRect = placeholder.AddComponent<RectTransform>();
        placeholderRect.sizeDelta = animalToMoveDown.GetComponent<RectTransform>().sizeDelta;
        placeholderRect.SetParent(animalsContainerPanelRect, false);

        LayoutRebuilder.ForceRebuildLayoutImmediate(animalsContainerPanelRect);

        Vector3 targetLocalPosition = placeholderRect.localPosition;



        Vector3 targetWorldPosition = animalsContainerPanelRect.TransformPoint(targetLocalPosition);

        Destroy(placeholder);
        if(animalToMoveDown != null)
        {

            animalToMoveDown.transform.DOKill();


            animalToMoveDown.transform.DOMove(targetWorldPosition, floatingAnimalAnimDuration)
                .SetEase(floatingAnimalEaseDown)
                .OnComplete(() =>
                {
                    if (animalToMoveDown != null && animalsContainerPanelRect != null)
                    {
                        //currentlyFloatingAnimalView = null;

                        animalToMoveDown.transform.SetParent(animalsContainerPanelRect, true);

                        animalToMoveDown.transform.SetAsLastSibling();

                        animalToMoveDown.transform.localScale = Vector3.one;

                 


                        LayoutRebuilder.ForceRebuildLayoutImmediate(animalsContainerPanelRect);
                        Vector3 finalLandedLocalPos = animalToMoveDown.transform.localPosition;

                        float bounceHeight = 13.0f;
                        float bounceDuration = 0.2f;
                        animalToMoveDown.transform.DOKill();
                        Sequence bounceSequence = DOTween.Sequence();

                        bounceSequence.Append(
                            animalToMoveDown.transform.DOLocalMoveY(finalLandedLocalPos.y + bounceHeight, bounceDuration / 2)
                            .SetEase(Ease.OutSine)
                        );

                        bounceSequence.Append(
                            animalToMoveDown.transform.DOLocalMoveY(finalLandedLocalPos.y, bounceDuration / 2)
                            .SetEase(Ease.InSine)
                        );

                        string bounceID = $"bounce_{animalToMoveDown.GetInstanceID()}";
                        bounceSequence.SetId(bounceID);
                        bounceSequence.Play();
                   
                        //if (lastAnimal && onCompleteCallback != null)
                        //    onCompleteCallback.Invoke();
                        float squashDuration = 0.15f;
                        Vector3 squashScale = new Vector3(1.1f, 0.9f, 1f);

                        animalToMoveDown.transform.DOKill();

                        Sequence squashSequence = DOTween.Sequence();

                        squashSequence.Append(
                            animalToMoveDown.transform.DOScale(squashScale, squashDuration)
                            .SetEase(Ease.OutQuad)
                        );

                        squashSequence.Append(
                            animalToMoveDown.transform.DOScale(Vector3.one, squashDuration)
                            .SetEase(Ease.InQuad)
                        );
                       

                        
                        squashSequence.SetId(bounceID);
                        squashSequence.Play();

                        if (lastAnimal && onCompleteCallback != null)
                            onCompleteCallback.Invoke();


                    }

                });
        }
                   
    }
  
    public void ShowEffect()
    {
        if (animalViewInstances.Count == tubeHeight)
        {
            AnimalView animalViewTop = animalViewInstances[^1];
            AnimalType animalType = animalViewTop.animaltype;
            bool checkSameType = true;
            foreach (AnimalView animalView in animalViewInstances)
            {
                if (animalType != animalView.animaltype)
                {
                    checkSameType = false;
                }
            }
            if (checkSameType)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(animalsContainerPanelRect);
                AnimalView bottomAnimalView = null;
                if (animalViewInstances.Count > 0)
                {
                    bottomAnimalView = animalViewInstances[0];
                }
                Vector3 effectSpawnPosition = new Vector3(bottomAnimalView.transform.position.x, bottomAnimalView.transform.position.y - 0.5f, bottomAnimalView.transform.position.z);
                EffectManager.Instance.PlayTubeCompleteEffectAtWorldPosition(effectSpawnPosition);

                foreach (AnimalView animalView in animalViewInstances)
                {
                    animalView.PlayCelebrationShake();
                }
            }
        }

    }

    public Vector3 GetEffectSpawnPosition()
    {

        LayoutRebuilder.ForceRebuildLayoutImmediate(animalsContainerPanelRect);
        AnimalView bottomAnimalView = null;
        bottomAnimalView = animalViewInstances[0];

        return new Vector3(bottomAnimalView.transform.position.x, bottomAnimalView.transform.position.y - 0.5f, bottomAnimalView.transform.position.z);

    }
    public void GetEffectAnimals()
    {

        foreach (AnimalView animalView in animalViewInstances)
        {
            animalView.PlayCelebrationShake();
        }

    }
    public bool CheckEffectSpawnPosition()
    {
        bool checkEffect = true;
        if (animalViewInstances.Count != tubeHeight) return false;


        AnimalView animalViewTop = animalViewInstances[^1];
        AnimalType animalType = animalViewTop.animaltype;
        foreach (AnimalView animalView in animalViewInstances)
        {
            if (animalType != animalView.animaltype)
            {
                checkEffect = false;
            }
        }


        return checkEffect;

    }
    void OnDestroy()
    {      
        StopAllCoroutines();

    }
}
