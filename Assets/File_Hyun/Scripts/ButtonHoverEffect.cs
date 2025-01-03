using DG.Tweening;
using UnityEngine;

public class ButtonHoverEffect : MonoBehaviour
{
    public GameObject[] targetObjects;
    public GameObject targetObject;

    private float animationDuration = 0.1f;

    private Vector2 initialScale = new Vector2(0.05f, 0.05f);
    private Vector2 targetScale = new Vector2(0.2f, 0.2f);
    private Vector2 clickScale = new Vector2(0.15f, 0.15f);
    private CanvasGroup[] canvasGroups;
    private CanvasGroup targetCanvasGroup;

    private void Start()
    {
        canvasGroups = new CanvasGroup[targetObjects.Length];

        // 각 오브젝트 초기화
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

            targetCanvasGroup.alpha = 0; // 초기 상태에서 투명하게 설정
        }

    }

    public void OnMouseEnter()
    {
        StopAllCoroutines();
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
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
            {
                targetObjects[i].transform.localScale = targetScale;
            }
        }

        if (targetObject != null)
        {
            // 타겟 오브젝트 애니메이션 실행
            targetObject.transform.localScale = new Vector2(0.05f, 0.01f);
            targetCanvasGroup.alpha = 1; // 애니메이션 실행 시 보이도록 설정

            Sequence sequence = DOTween.Sequence();
            sequence.Append(targetObject.transform.DOScale(new Vector2(0.05f, 1f), animationDuration).SetEase(Ease.OutQuad));
            sequence.Append(targetObject.transform.DOScale(new Vector2(1f, 1f), animationDuration).SetEase(Ease.OutQuad));

            // 애니메이션 종료 후 투명도 감소
            sequence.AppendInterval(animationDuration) // 잠시 대기
                   .Append(targetCanvasGroup.DOFade(0f, animationDuration)) // 투명도 감소
                   .OnComplete(() => targetObject.transform.localScale = initialScale); // 스케일 초기화
        }
    }
}