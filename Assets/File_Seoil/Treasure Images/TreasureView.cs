using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreasureView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector2 defaultPosition;
    [SerializeField] private GameObject descriptionPrefab;

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

        currentDescription.Description.text = descriptionText;

        currentDescription.gameObject.GetComponent<RectTransform>().anchoredPosition = defaultPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(currentDescription != null) Destroy(currentDescription.gameObject);
    }
}
