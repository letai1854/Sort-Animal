using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    [Header("Particle Prefabs")]
    public GameObject tubeCompleteLauncherPrefab; 
    public GameObject tubeCompleteLauncherTrailPrefab;
    public GameObject effectFireWorkWin;
    public Canvas mainUICanvas;
    public int particleCanvasSortOrder = 10;
    public GameObject upArrow;

    [Header("Arrow Prefabs")]
    public float arrowBounceHeight = 23f; 
    public float arrowSingleBounceDuration = 0.3f; 
    public float arrowFadeDuration = 0.3f; 
    public Ease arrowBounceEase = Ease.InOutSine;

    private GameObject activeArrowInstance;
    private Sequence arrowAnimationSequence;

    [Header("Notify Prefabs")]
    public GameObject notifyPrefab;
    public float notifyMoveDuration = 0.2f;
    public float notifyFadeDuration = 0.3f;
    public Ease notifyEase = Ease.InOutSine;
    private GameObject activeNotificationInstance;
    private Sequence notificationSequence;


    private Coroutine fireworkLoopCoroutine;
    private GameObject activeFireworkInstance;

    bool checkExplore = false;
    public void PlayTubeCompleteEffectAtWorldPosition(Vector3 worldSpawnPosition)
    {
        if (tubeCompleteLauncherPrefab != null)
        {
            SpawnAndPlayParticleInUICanvas(tubeCompleteLauncherPrefab, worldSpawnPosition);
        }
        if (tubeCompleteLauncherTrailPrefab != null) { 
        
            SpawnAndPlayParticleInUICanvas(tubeCompleteLauncherTrailPrefab, worldSpawnPosition);
        }
    }


    private void SpawnAndPlayParticleInUICanvas(GameObject particlePrefab, Vector3 worldPos)
    {
        if (particlePrefab == null || mainUICanvas == null) return;

        GameObject canvasGO = new GameObject(particlePrefab.name + "_FXCanvas");
        canvasGO.transform.position = worldPos;
        canvasGO.transform.SetParent(mainUICanvas.transform, true);


     

        GameObject psInstanceGO = Instantiate(particlePrefab, Vector3.zero, Quaternion.identity);
        Vector3 targetTransform = new Vector3(canvasGO.transform.position.x, canvasGO.transform.position.y - 5f, canvasGO.transform.position.z);
        psInstanceGO.transform.SetParent(canvasGO.transform, false);
        psInstanceGO.transform.rotation = Quaternion.LookRotation(Vector3.up, -Camera.main.transform.forward);
        ParticleSystem ps = psInstanceGO.GetComponent<ParticleSystem>();
        float maxLifetime = 3.0f;
        if (ps != null) 
        Destroy(canvasGO, maxLifetime + 1.0f);
    }

    public void RandomEffectFireWorkWin()
    {
        StartCoroutine(RandomEffectFireWorkCoroutine());
    }

    private IEnumerator RandomEffectFireWorkCoroutine()
    {
        Vector3 indexLeftUp = new Vector3(mainUICanvas.transform.position.x - 350f, mainUICanvas.transform.position.y + 600f);
        Vector3 indexRightUp = new Vector3(mainUICanvas.transform.position.x + 350f, mainUICanvas.transform.position.y + 600f);
        Vector3 indexLeftDown = new Vector3(mainUICanvas.transform.position.x - 350f, mainUICanvas.transform.position.y);
        Vector3 indexRightDown = new Vector3(mainUICanvas.transform.position.x + 350f, mainUICanvas.transform.position.y);

        Vector3[] positions = new Vector3[] { indexLeftUp, indexRightUp, indexLeftDown, indexRightDown };

        GameObject psInstanceGO = Instantiate(effectFireWorkWin, indexLeftUp, Quaternion.identity);
        psInstanceGO.transform.SetParent(mainUICanvas.transform, false);
        psInstanceGO.GetComponent<ParticleSystem>().Pause();

        int index = 0;
        checkExplore = false;

        while (true) // Lặp vô hạn nhưng không block main thread
        {
            if (!checkExplore)
            {
                checkExplore = true;
                Vector3 positionIndex = positions[index];
                StartCoroutine(ShowPopupWinCoroutin(psInstanceGO, positionIndex));
                index = (index + 1) % positions.Length;
            }

            yield return null; // rất quan trọng: tránh freeze game
        }
    }

    private IEnumerator ShowPopupWinCoroutin(GameObject prefabsWin, Vector3 index)
    {
        prefabsWin.transform.localPosition = index;

        yield return new WaitForSeconds(0.2f);
        prefabsWin.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(1f);
        prefabsWin.GetComponent<ParticleSystem>().Pause();

        yield return new WaitForSeconds(2f);
        checkExplore = false;
    }


    public void ArrowEffect(TubeView tube)
    {

        if (activeArrowInstance != null)
        {
            arrowAnimationSequence?.Kill();
            Destroy(activeArrowInstance);
            activeArrowInstance = null;
        }

        activeArrowInstance = Instantiate(upArrow);
        activeArrowInstance.transform.SetParent (tube.transform, false);
        //activeArrowInstance.transform.position = new Vector3(0, 100f, 0);


        Image arrowImage = activeArrowInstance.GetComponentInChildren<Image>();
        Vector3 desiredLocalPosition = tube.floatingAnimalPositionOffset + new Vector3(0, 50f, 0);
        activeArrowInstance.transform.localPosition = desiredLocalPosition;

        if (arrowImage == null)
        {
            Debug.LogWarning("Arrow instance is missing Image component (or in children). Fade effect might not work.");
        }

        arrowAnimationSequence = DOTween.Sequence();

        Vector3 lowerBouncePosition = desiredLocalPosition - new Vector3(0, arrowBounceHeight+50f, 0);

        arrowAnimationSequence
            .AppendCallback(() => {
                if (arrowImage != null) arrowImage.DOFade(1f, arrowFadeDuration); 
            })
            .Append(activeArrowInstance.transform.DOLocalMove(lowerBouncePosition, arrowSingleBounceDuration).SetEase(arrowBounceEase))
            .AppendInterval(0.1f)
            .AppendCallback(() => {
                if (arrowImage != null) arrowImage.DOFade(0f, 0.5f); 
            })
            .Append(activeArrowInstance.transform.DOLocalMove(desiredLocalPosition, arrowSingleBounceDuration).SetEase(arrowBounceEase))
            .AppendInterval(0.1f)
            .AppendInterval(0.3f)
            .SetLoops(-1, LoopType.Restart);

        arrowAnimationSequence.Play();

    }
    public void StopArrowEffect()
    {
        arrowAnimationSequence?.Kill();
        if (activeArrowInstance != null)
        {
            Destroy(activeArrowInstance);
            activeArrowInstance = null;
        }
    }
    public void NotifyUndo() // Thêm tham số message
    {
        // Dừng và hủy notification cũ
        if (activeNotificationInstance != null)
        {
            notificationSequence?.Kill();
            Destroy(activeNotificationInstance);
            activeNotificationInstance = null;
        }

        if (notifyPrefab == null || mainUICanvas == null)
        {
            Debug.LogError("NotifyUndo: Notify Prefab or Main UI Canvas is not assigned!");
            return;
        }

        activeNotificationInstance = Instantiate(notifyPrefab);
        activeNotificationInstance.SetActive(true); // Mặc định là true

        // Đặt parent và reset local transform
        activeNotificationInstance.transform.SetParent(mainUICanvas.transform, false);
        // Sau dòng trên, activeNotificationInstance.transform.localPosition là (0,0,0)
        // (hoặc gần đó, tùy thuộc vào RectTransform của nó và parent)
        // và nó sẽ nằm ở pivot của mainUICanvas.

        // --- Thiết lập vị trí ban đầu và các vị trí đích (TẤT CẢ LÀ LOCAL POSITIONS so với mainUICanvas) ---

        // 1. Vị trí xuất hiện ban đầu (ví dụ: giữa màn hình theo chiều ngang, hơi thấp một chút)
        RectTransform canvasRect = mainUICanvas.GetComponent<RectTransform>();
        float initialYOffset = canvasRect.rect.height * 0.2f; // Ví dụ: 30% từ đáy lên, tính từ giữa nếu pivot là 0.5
                                                               // Hoặc một giá trị cố định nếu bạn muốn: new Vector3(0, -200f, 0);
        Vector3 startLocalPosition = new Vector3(0, initialYOffset, 0);
        activeNotificationInstance.transform.localPosition = startLocalPosition;


        float riseAmount = 2f; 
        Vector3 mediumLocalPosition = startLocalPosition + new Vector3(0, riseAmount, 0);

        float furtherRiseAmount = 300f; 
        Vector3 highLocalPosition = mediumLocalPosition + new Vector3(0, furtherRiseAmount, 0);


        CanvasGroup canvasGroup = activeNotificationInstance.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = activeNotificationInstance.AddComponent<CanvasGroup>(); 
        }
        canvasGroup.alpha = 0f; 

        notificationSequence = DOTween.Sequence();

        notificationSequence.Append(canvasGroup.DOFade(1f, notifyFadeDuration));
        notificationSequence.Append(activeNotificationInstance.transform.DOLocalMove(mediumLocalPosition, 0.1f)
            .SetEase(notifyEase));
        notificationSequence.AppendInterval(0.1f); 

        notificationSequence.Append(activeNotificationInstance.transform.DOLocalMove(highLocalPosition, 0.8f) 
            .SetEase(Ease.InQuad)); 


        notificationSequence.Insert(notificationSequence.Duration() - 0.8f + 0.4f, 
                                    canvasGroup.DOFade(0f, 0.55f)); 

        notificationSequence.OnComplete(() =>
        {
            if (activeNotificationInstance != null) 
            {
                Destroy(activeNotificationInstance);
            }
            activeNotificationInstance = null; 
            notificationSequence = null; 
        });

        notificationSequence.Play();
    }



    public void StartRandomFireworkWinLoop()
    {
        if (effectFireWorkWin == null || mainUICanvas == null)
        {
            Debug.LogError("EffectFireWorkWin Prefab or MainUICanvas is not assigned!");
            return;
        }

        if (fireworkLoopCoroutine != null)
        {
            StopCoroutine(fireworkLoopCoroutine);
            if (activeFireworkInstance != null) Destroy(activeFireworkInstance);
        }
        fireworkLoopCoroutine = StartCoroutine(RandomFireworkWinSequence());
    }

    public void StopRandomFireworkWinLoop()
    {
        if (fireworkLoopCoroutine != null)
        {
            StopCoroutine(fireworkLoopCoroutine);
            fireworkLoopCoroutine = null;
        }
        if (activeFireworkInstance != null)
        {
            Destroy(activeFireworkInstance);
            activeFireworkInstance = null;
        }
        Debug.Log("Random Firework Win Loop STOPPED.");
    }

    private IEnumerator RandomFireworkWinSequence()
    {
        Debug.Log("Random Firework Win Loop STARTED.");

        RectTransform canvasRect = mainUICanvas.GetComponent<RectTransform>();
        float halfWidth = canvasRect.rect.width / 2 * 0.7f; // 80% nửa chiều rộng
        float halfHeight = canvasRect.rect.height / 2 * 0.7f; // 80% nửa chiều cao

        Vector3[] localPositions = new Vector3[] {
            new Vector3(-halfWidth, halfHeight, 0), // Top-Left (local to canvas center if pivot is center)
            new Vector3(halfWidth, halfHeight, 0),  // Top-Right
            new Vector3(-halfWidth, -halfHeight, 0),// Bottom-Left
            new Vector3(halfWidth, -halfHeight, 0) // Bottom-Right
        };
       
        // Chuyển local positions thành world positions
        Vector3[] worldPositions = new Vector3[localPositions.Length];

        for (int i = 0; i < localPositions.Length; i++)
        {
            worldPositions[i] = localPositions[i];
        }

        activeFireworkInstance = Instantiate(effectFireWorkWin);
        activeFireworkInstance.transform.SetParent(mainUICanvas.transform, false); 
        activeFireworkInstance.SetActive(false);

        ParticleSystem ps = activeFireworkInstance.GetComponent<ParticleSystem>();
        if (ps == null)
        {
            Debug.LogError("effectFireWorkWin prefab is missing ParticleSystem component!");
            Destroy(activeFireworkInstance);
            fireworkLoopCoroutine = null;
            yield break;
        }
        var mainModule = ps.main; // Để dễ truy cập
        mainModule.stopAction = ParticleSystemStopAction.None; // Để có thể Play/Stop nhiều lần

        int index = 0;
        while (true)
        {
            activeFireworkInstance.transform.localPosition = worldPositions[index];
            activeFireworkInstance.SetActive(true);
            ps.Play(true); // true để restart và play


            yield return new WaitForSeconds(mainModule.duration + 1.3f); 

            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            activeFireworkInstance.SetActive(false);

            yield return new WaitForSeconds(1.5f); 

            index++;
            if (index >= worldPositions.Length)
            {
                index = 0;
            }
        }
    }

}
