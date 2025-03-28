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

        currentDescription.Description.text = descriptionText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentDescription.Destroy();
    }

    public void Destroy()
    {
        if (currentDescription != null) Destroy(currentDescription.gameObject);
    }
}
