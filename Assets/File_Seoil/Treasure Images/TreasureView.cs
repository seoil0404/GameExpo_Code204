using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreasureView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector2 defaultPosition;
    [SerializeField] private GameObject descriptionPrefab;

    [SerializeField] private float startScale;
    [SerializeField] private float endScale;
    [SerializeField] private float scaleDuration;

    private DescriptionView currentDescription;
    private string descriptionText;

    public string DescriptionText
    {
        set
        {
            descriptionText = value;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.SetAsLastSibling();

        currentDescription = Instantiate(descriptionPrefab, transform).GetComponent<DescriptionView>();

        currentDescription.transform.localScale = Vector3.one * startScale;
        currentDescription.transform.DOScale(Vector3.one * endScale, scaleDuration);

        currentDescription.Description.text = descriptionText;

        currentDescription.gameObject.GetComponent<RectTransform>().anchoredPosition = defaultPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(currentDescription != null) Destroy(currentDescription.gameObject);
    }
}
