using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public bool IsAllowClick = false;

    private Vector3 currentScale;

    private void Awake()
    {
        currentScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(IsAllowClick)
        {
            transform.DOKill();
            transform.DOScale(currentScale * 1.5f, 0.25f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(currentScale, 0.25f);
    }
}
