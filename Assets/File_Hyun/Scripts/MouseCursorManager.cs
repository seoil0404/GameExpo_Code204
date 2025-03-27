using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    public GameObject targetObject;

    public Sprite idleSprite;
    public Sprite pressedSprite;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
#if !UNITY_EDITOR
        Cursor.visible = false;
#endif

        if (targetObject != null)
        {
            spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        targetObject.transform.position = mouseWorldPosition;

        if (Input.GetMouseButton(0))
        {
            if (pressedSprite != null)
                spriteRenderer.sprite = pressedSprite;
        }
        else
        {
            if (idleSprite != null)
                spriteRenderer.sprite = idleSprite;
        }
    }
}
