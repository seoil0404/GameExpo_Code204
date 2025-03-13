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
        if (GetComponentInParent<Canvas>().renderMode == RenderMode.ScreenSpaceCamera)
        {
            if (transform.position.x > 0) 
                FlipDescription();
        }
        else if (GetComponentInParent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay)
        {
            if (RectTransformUtility.WorldToScreenPoint(null, transform.position).x > GetComponentInParent<CanvasScaler>().referenceResolution.x / 2)
                FlipDescription();
        }
        else
        {
            if (transform.position.x > 0)
                FlipDescription();
        }
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
