using UnityEngine;
using UnityEngine.UI;

public class DescriptionView : MonoBehaviour
{
    public Text Description;

    private void Awake()
    {
        HandleFlip();
    }

    private void HandleFlip()
    {
        if (transform.position.x > Screen.width / 2)
            FlipDescription();

        Debug.Log(RectTransformUtility.WorldToScreenPoint(null, transform.position).x + " " + GetComponentInParent<CanvasScaler>().referenceResolution.x / 2);
    }

    private void FlipDescription()
    {
        transform.localScale = new(
                    transform.localScale.x * -1,
                    transform.localScale.y,
                    transform.localScale.z
                    );

        Description.transform.localScale = new(
            Description.transform.localScale.x * -1,
            Description.transform.localScale.y,
            Description.transform.localScale.z
            );
    }
}
