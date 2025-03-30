using DG.Tweening;
using UnityEngine;

public class ButtonHoverEffect : MonoBehaviour
{
    public GameObject[] targetObjects;
    public GameObject targetObject;
    
    public AudioClip onMouseClip;
    public AudioClip clickClip;

    private float animationDuration = 0.1f;

    private Vector2 initialScale = new Vector2(0.05f, 0.05f);
    private Vector2 targetScale = new Vector2(0.2f, 0.2f);
    private Vector2 clickScale = new Vector2(0.15f, 0.15f);
    private CanvasGroup[] canvasGroups;
    private CanvasGroup targetCanvasGroup;
    private AudioSource audioSource;

    private void Start()
    {
        canvasGroups = new CanvasGroup[targetObjects.Length];
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                targetObjects[i].transform.localScale = targetScale;

                canvasGroups[i] = targetObjects[i].GetComponent<CanvasGroup>();
                if (canvasGroups[i] == null)
                {
                    canvasGroups[i] = targetObjects[i].AddComponent<CanvasGroup>();
                }
            }
        }

        StopAllCoroutines();

        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                targetObjects[i].transform.DOKill();
                canvasGroups[i].DOKill();

                targetObjects[i].transform.localScale = initialScale;
                canvasGroups[i].alpha = 0f;
            }
        }

        if (targetObject != null)
        {
            targetCanvasGroup = targetObject.GetComponent<CanvasGroup>();
            if (targetCanvasGroup == null)
            {
                targetCanvasGroup = targetObject.AddComponent<CanvasGroup>();
            }

            targetCanvasGroup.alpha = 0;
        }

        RemoveEffect();
    }

    public void OnMouseEnter()
    {
        StopAllCoroutines();

        PlaySound(onMouseClip);
        
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                targetObjects[i].transform.DOKill();
                canvasGroups[i].DOKill();
                
                targetObjects[i].transform.DOScale(targetScale, animationDuration).SetEase(Ease.OutQuad);
                
                canvasGroups[i].DOFade(1f, animationDuration);
            }
        }
    }

    public void OnMouseExit()
    {
        StopAllCoroutines();
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                targetObjects[i].transform.DOKill();
                canvasGroups[i].DOKill();
                
                targetObjects[i].transform.DOScale(initialScale, animationDuration).SetEase(Ease.InQuad);
                
                canvasGroups[i].DOFade(0f, animationDuration);
            }
        }
    }

    public void OnPointerDown()
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                targetObjects[i].transform.localScale = clickScale;
            }
        }
    }

    public void OnPointerUp()
    {
        PlaySound(clickClip);
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                targetObjects[i].transform.localScale = targetScale;
            }
        }

        if (targetObject != null)
        {
            targetObject.transform.localScale = new Vector2(0.05f, 0.01f);
            targetCanvasGroup.alpha = 1;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(targetObject.transform.DOScale(new Vector2(0.05f, 1f), animationDuration).SetEase(Ease.OutQuad));
            sequence.Append(targetObject.transform.DOScale(new Vector2(1f, 1f), animationDuration).SetEase(Ease.OutQuad));

            sequence.AppendInterval(animationDuration)
                   .Append(targetCanvasGroup.DOFade(0f, animationDuration))
                   .OnComplete(() => targetObject.transform.localScale = initialScale);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or AudioClip is missing!");
        }
    }

    public void RemoveEffect()
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                targetObjects[i].transform.DOKill();
                canvasGroups[i].DOKill();

                targetObjects[i].transform.localScale = initialScale;
                canvasGroups[i].alpha = 0f;
            }
        }
    }

    void OnDisable()
    {
        RemoveEffect();
    }
}