using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimalView : MonoBehaviour
{
    [System.Serializable]
    public class ListanimalSprites
{
    public AnimalHabitatType animalTypeName;
    public List<AnimalSpriteMapping> animalSprites;
}
    public Image animalImage;
    public AnimalType animaltype;
    [System.Serializable]
    public struct AnimalSpriteMapping
    {
        public AnimalType type;
        public Sprite sprite;
    }
    public List<AnimalSpriteMapping> animalSprites;

    public List<ListanimalSprites> listanimalSprites; 

    [Header("Animation Settings")]
    public float rotationAngle = 15f; 
    public float rotationDuration = 0.5f; 
    public float delayBetweenRotations = 2f; 
    public Ease rotationEase = Ease.InOutSine; 

    private Sequence idleAnimationSequence;


    [Header("Effects")]
    [Tooltip("Prefab của hiệu ứng vệt mờ. Sẽ được Instantiate khi cần.")]
    [SerializeField] private ParticleSystem ghostTrailPrefab;



    [Header("Celebration Animation")]
    public float celebrationShakeDuration = 1.2f; 
    public float celebrationShakeStrength = 40f; 
    public int celebrationShakeVibrato = 30;    
    public float celebrationShakeRandomness = 100f; 
    public Ease celebrationEase = Ease.OutElastic;
    private Sequence _celebrationSequence;
    private void Start()
    {
        if (animalImage != null && animalImage.enabled)
        {
            StartIdleAnimation();
        }
    }
    public void SetAnimal(AnimalType type)
    {
        if (animalSprites == null)
        {
            return;
        }
        animaltype = type;
        Sprite sprite = GetSpriteForType(type);
        if (sprite != null) 
        {
            animalImage.sprite = sprite;
            animalImage.enabled = true;
            StartIdleAnimation();
            
        }
        else
        {
            animalImage.enabled= false;
        }

    }

    public void SetAnimal(AnimalType type, AnimalHabitatType activeTheme) 
    {
    

        animaltype = type; 

        ListanimalSprites currentThemeSprites = null;
        foreach (var themeCollection in listanimalSprites)
        {
            if (themeCollection.animalTypeName == activeTheme)
            {
                currentThemeSprites = themeCollection;
                break;
            }
        }

        if (animalSprites == null)
        {
            return;
        }
        if(currentThemeSprites != null)
        {
            animalSprites = currentThemeSprites.animalSprites;
        }
        animaltype = type;
        Sprite sprite = GetSpriteForType(type);
        if (sprite != null)
        {
            animalImage.sprite = sprite;
            animalImage.enabled = true;
            StartIdleAnimation();

        }
        else
        {
            animalImage.enabled = false;
        }
    }

  

    private Sprite GetSpriteForType(AnimalType type)
    {
        foreach (var sprite in animalSprites) 
        {
            if(sprite.type == type)
            {
                return sprite.sprite;
            }
        }
        return null;
    }
    void StartIdleAnimation()
    {
        if (animalImage == null || !animalImage.enabled) return;

        if (idleAnimationSequence != null && idleAnimationSequence.IsActive())
        {
            idleAnimationSequence.Kill();
        }
        animalImage.rectTransform.localRotation = Quaternion.identity; 

        idleAnimationSequence = DOTween.Sequence();

        float singleRotationTime = rotationDuration; 

        float initialDelay = Random.Range(0f, 0.5f); 

        idleAnimationSequence
            .AppendInterval(initialDelay) 
                                          
            .Append(animalImage.rectTransform.DOLocalRotate(new Vector3(0, 0, rotationAngle), singleRotationTime).SetEase(rotationEase)) 
            .AppendInterval(Random.Range(0.05f, 0.15f))
            .Append(animalImage.rectTransform.DOLocalRotate(Vector3.zero, singleRotationTime).SetEase(rotationEase)) 
            .AppendInterval(Random.Range(delayBetweenRotations * 0.4f, delayBetweenRotations * 0.6f)) 
            .Append(animalImage.rectTransform.DOLocalRotate(new Vector3(0, 0, -rotationAngle), singleRotationTime).SetEase(rotationEase)) 
            .AppendInterval(Random.Range(0.05f, 0.15f))
            .Append(animalImage.rectTransform.DOLocalRotate(Vector3.zero, singleRotationTime).SetEase(rotationEase)) 
            .AppendInterval(Random.Range(delayBetweenRotations * 0.4f, delayBetweenRotations * 0.6f)); 

        idleAnimationSequence.SetLoops(-1, LoopType.Restart);
        idleAnimationSequence.Play();
    }
    void StopIdleAnimation()
    {
        if (idleAnimationSequence != null && idleAnimationSequence.IsActive())
        {
            idleAnimationSequence.Kill(); 
        }
        idleAnimationSequence = null; 

        if (animalImage != null && animalImage.rectTransform != null)
        {
            animalImage.rectTransform.localRotation = Quaternion.identity; 
        }
    }
    void OnEnable()
    {
        if (animalImage != null && animalImage.enabled && animalImage.sprite != null)
        {
            StartIdleAnimation();
        }
    }

    void OnDisable()
    {
        StopIdleAnimation();
    }

    void OnDestroy()
    {
        StopIdleAnimation();
    }
   

    public void PlayCelebrationShake()
    {
        StopIdleAnimation(); 
  

        _celebrationSequence?.Kill();
        animalImage.rectTransform.localRotation = Quaternion.identity;

        _celebrationSequence = DOTween.Sequence();
        _celebrationSequence.Append(
            animalImage.rectTransform.DOShakeRotation(celebrationShakeDuration, new Vector3(0, 0, celebrationShakeStrength), celebrationShakeVibrato, celebrationShakeRandomness)
                .SetEase(celebrationEase)
        );
     

        _celebrationSequence.OnComplete(() => {
    
            animalImage.rectTransform.localRotation = Quaternion.identity;
            ReturnToIdleAfterCelebration();
        });

        _celebrationSequence.Play();
    }

    public void ReturnToIdleAfterCelebration()
    {
        _celebrationSequence?.Kill(true); 
        animalImage.rectTransform.localRotation = Quaternion.identity;
        StartIdleAnimation();
    }

}
