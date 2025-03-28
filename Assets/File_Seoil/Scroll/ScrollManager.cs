using Scroll;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private ScrollData scrollData;
    [SerializeField] private CombatData combatData;

    [Header("MonoBehavior")]
    [SerializeField] private Image slot1Image;
    [SerializeField] private ScrollView slot1ScrollView;
    [SerializeField] private Image slot2Image;
    [SerializeField] private ScrollView slot2ScrollView;
    [SerializeField] private Image slot3Image;
    [SerializeField] private ScrollView slot3ScrollView;

    private void Awake()
    {
        SyncScroll();
    }

    public void OnUseSlot1()
    {
        if(scrollData.Slot1 != ScrollData.ScrollType.None) OnUse(ref scrollData.Slot1);
    }

    public void OnUseSlot2()
    {
        if (scrollData.Slot2 != ScrollData.ScrollType.None) OnUse(ref scrollData.Slot2);
    }

    public void OnUseSlot3()
    {
        if (scrollData.Slot3 != ScrollData.ScrollType.None) OnUse(ref scrollData.Slot3);
    }

    private void OnUse(ref ScrollData.ScrollType type)
    {
        switch (type)
        {
            case ScrollData.ScrollType.ReStart:
                break;
            case ScrollData.ScrollType.Delete:
                break;
            case ScrollData.ScrollType.Speed:
                break;
            case ScrollData.ScrollType.FireBall:
                break;
            case ScrollData.ScrollType.Curse:
                break;
            case ScrollData.ScrollType.Strengh:
                break;
            case ScrollData.ScrollType.Energy:
                break;
            case ScrollData.ScrollType.Poision:
                break;
            case ScrollData.ScrollType.Heal:
                break;
            case ScrollData.ScrollType.Reflection:
                break;
            case ScrollData.ScrollType.Escape:
                if (combatData.EnemyType != EnemyData.EnemyType.Boss)
                {

                }
                else return;
                break;
            case ScrollData.ScrollType.Money:
                break;
            case ScrollData.ScrollType.Grow:
                break;
            case ScrollData.ScrollType.FirePillar:
                break;
            case ScrollData.ScrollType.Fill:
                break;
            case ScrollData.ScrollType.Life:
                break;
        }

        type = ScrollData.ScrollType.None;
        SyncScroll();
    }

    private void SyncScroll()
    {
        slot1Image.sprite = scrollData.GetImage(scrollData.Slot1);
        slot2Image.sprite = scrollData.GetImage(scrollData.Slot2);
        slot3Image.sprite = scrollData.GetImage(scrollData.Slot3);

        Color transparentColor = Color.white;
        transparentColor.a = 0;

        if (slot1Image.sprite == null) slot1Image.color = transparentColor;
        if (slot2Image.sprite == null) slot2Image.color = transparentColor;
        if (slot3Image.sprite == null) slot3Image.color = transparentColor;

        slot1ScrollView.scrollType = scrollData.Slot1;
        slot2ScrollView.scrollType = scrollData.Slot2;
        slot3ScrollView.scrollType = scrollData.Slot3;
    }
}
