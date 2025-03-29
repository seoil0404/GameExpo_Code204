using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionView : MonoBehaviour
{
    public Text Description;

    [SerializeField] private Vector2 defaultScale;
    [SerializeField] private float startScale;
    [SerializeField] private float endScale;
    [SerializeField] private Vector2 defaultPosition;
    [SerializeField] private float scaleDuration;
 
    private void Show()
    {
        Vector3 tempScale = transform.localScale;

        transform.localScale = tempScale * startScale;
        
        transform.DOScale(tempScale * endScale, scaleDuration);

        GetComponent<RectTransform>().anchoredPosition = defaultPosition;

        Scene.Controller.OnLoadScene += Destroy;
    }

    private void OnDisable()
    {
        Scene.Controller.OnLoadScene -= Destroy;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        NormalizeScale();
        HandleFlip();
        Show();
    }

    private void NormalizeScale()
    {
        Vector2 parentScale = transform.parent.parent.parent.localScale;

        Debug.Log(parentScale.ToString());

        transform.localScale = new Vector2(
            defaultScale.x,
            defaultScale.y
        );
    }

    private void HandleFlip()
    {
        if (transform.position.x > Screen.width / 2)
            FlipDescriptionX();

        if(transform.position.y < Screen.height / 2)
            FlipDescriptionY();
    }

    private void FlipDescriptionX()
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

    private void FlipDescriptionY()
    {
        transform.localScale = new(
                    transform.localScale.x,
                    transform.localScale.y * -1,
                    transform.localScale.z
                    );

        Description.transform.localScale = new(
            Description.transform.localScale.x,
            Description.transform.localScale.y * -1,
            Description.transform.localScale.z
            );
    }
}
