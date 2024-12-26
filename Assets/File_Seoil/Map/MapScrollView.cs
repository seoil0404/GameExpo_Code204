using Unity.VisualScripting;
using UnityEngine;

public class MapScrollView : MonoBehaviour
{
    [SerializeField] private float sensitivity;

    private float toPos;

    private RectTransform rectTransform;

    public float maxValue;
    public float minValue;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        toPos = rectTransform.anchoredPosition.y;
    }

    private void Update()
    {
        HandleScroll();
    }

    private void HandleScroll()
    {
        float scrollRate = -Input.GetAxis("Mouse ScrollWheel");
        
        scrollRate *= sensitivity;
        
        if (toPos + scrollRate > maxValue || toPos + scrollRate < minValue)
        {
            return;
        }

        toPos += scrollRate;

        rectTransform.anchoredPosition = Vector3.Lerp(
            rectTransform.anchoredPosition,
            new Vector2(rectTransform.anchoredPosition.x, toPos),
            Time.deltaTime * 10
        );
    }
}
