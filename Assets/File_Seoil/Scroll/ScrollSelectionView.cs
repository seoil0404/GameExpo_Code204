using Scroll;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSelectionView : MonoBehaviour
{
    [HideInInspector] private ScrollData.ScrollType newScrollType;
    [SerializeField] private ScrollData scrollData;

    [Header("MonoBehavior")]
    [SerializeField] private Image slot1Image;
    [SerializeField] private ScrollView slot1ScrollView;
    [SerializeField] private Image slot2Image;
    [SerializeField] private ScrollView slot2ScrollView;
    [SerializeField] private Image slot3Image;
    [SerializeField] private ScrollView slot3ScrollView;

    public ScrollData.ScrollType NewScrollType
    {
        set
        {
            newScrollType = value;
        }
    }

    private void Awake()
    {
        slot1Image.sprite = scrollData.GetImage(scrollData.Slot1);
        slot2Image.sprite = scrollData.GetImage(scrollData.Slot2);
        slot3Image.sprite = scrollData.GetImage(scrollData.Slot3);

        Color transparentColor = Color.white;
        transparentColor.a = 0;

        if (slot1Image.sprite == null) slot1Image.color = transparentColor;
        else slot1Image.color = Color.white;

        if (slot2Image.sprite == null) slot2Image.color = transparentColor;
        else slot2Image.color = Color.white;

        if (slot3Image.sprite == null) slot3Image.color = transparentColor;
        else slot3Image.color = Color.white;

        slot1ScrollView.scrollType = scrollData.Slot1;
        slot2ScrollView.scrollType = scrollData.Slot2;
        slot3ScrollView.scrollType = scrollData.Slot3;
    }

    public void OnClickSlot1()
    {
        scrollData.Slot1 = newScrollType;
        Destroy();
    }

    public void OnClickSlot2()
    {
        scrollData.Slot2 = newScrollType;
        Destroy();
    }

    public void OnClickSlot3()
    {
        scrollData.Slot3 = newScrollType;
        Destroy();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
