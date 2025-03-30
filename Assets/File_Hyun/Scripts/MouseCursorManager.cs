using UnityEngine;
using UnityEngine.UI;

public class MouseCursorManager : MonoBehaviour
{
    public RectTransform cursorRectTransform;
    public Image cursorImage;

    public Sprite idleSprite;
    public Sprite pressedSprite;

    void Start()
    {
#if !UNITY_EDITOR
        Cursor.visible = false;
#endif
    }

    void Update()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            cursorRectTransform.parent as RectTransform,
            Input.mousePosition,
            null,
            out mousePos
        );

        cursorRectTransform.anchoredPosition = mousePos;

        if (Input.GetMouseButton(0))
        {
            if (pressedSprite != null)
                cursorImage.sprite = pressedSprite;
            cursorRectTransform.localScale = new(0.8f, 0.8f, 0.8f);
        }
        else
        {
            if (idleSprite != null)
                cursorImage.sprite = idleSprite;
            cursorRectTransform.localScale = new(0.9f, 0.9f, 0.9f);
        }
    }
}
