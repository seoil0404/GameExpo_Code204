using Scroll;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

    public bool OnUseLifeScroll()
    {
        bool isSlot1Exist = scrollData.Slot1 == ScrollData.ScrollType.Life;
        bool isSlot2Exist = scrollData.Slot2 == ScrollData.ScrollType.Life;
        bool isSlot3Exist = scrollData.Slot3 == ScrollData.ScrollType.Life;

        if (!(isSlot1Exist || isSlot2Exist || isSlot3Exist)) return false;

        if (isSlot1Exist) scrollData.Slot1 = ScrollData.ScrollType.None;
        else if (isSlot2Exist) scrollData.Slot2 = ScrollData.ScrollType.None;
        else if (isSlot3Exist) scrollData.Slot3 = ScrollData.ScrollType.None;

        //Write Life Scroll Second Function
        {

        }

        return true;
    }

    private void OnUse(ref ScrollData.ScrollType type)
    {
        GameObject selectedEnemy = Grid.instance.GetSelectedEnemy();
        GameObject enemy = Grid.instance.GetSelectedEnemy();
        EnemyStats stats = enemy.GetComponent<EnemyStats>();
        var treasureEffect = Object.FindFirstObjectByType<TreasureEffect>();

        if (treasureEffect.GoldAndSilver)
        {
            CharacterManager.instance.GetGold(8);
        }

        if(treasureEffect.MultipleCureScroll)
        {
            CharacterManager.instance.RecoverHp(3);
        }

        switch (type)
        {
            case ScrollData.ScrollType.ReStart:
                FindFirstObjectByType<ShapeStorage>()?.RedrawOneShape();
                break;
            case ScrollData.ScrollType.Delete:
                FindFirstObjectByType<HoldShape>()?.ForceClearHeldShape();
                break;
            case ScrollData.ScrollType.Speed:
                CharacterManager.instance.ApplyDodgeBuff(1);
                break;
            case ScrollData.ScrollType.FireBall:
                selectedEnemy.GetComponent<EnemyStats>().TakeFixedDamage(10);
                break;
            case ScrollData.ScrollType.Curse:
                if (selectedEnemy != null)
                {
                    selectedEnemy.GetComponent<EnemyStats>().ApplyHealingReduction(3);
                }
                break;
            case ScrollData.ScrollType.Strengh:
                stats.DeactivateDamageMultiplier();
               
                break;
            case ScrollData.ScrollType.Energy:
                if (enemy != null)
                {
                    if (stats != null)
                    {
                        stats.atk3UpTurnCount += 1;
                    }
                }
                break;
            case ScrollData.ScrollType.Poision:
                if (selectedEnemy != null)
                {
                    selectedEnemy.GetComponent<EnemyStats>().ApplyPoisonFromPlayer(5);
                }
                break;
            case ScrollData.ScrollType.Heal:
                CharacterManager.instance.HealByPercentageOfMaxHp(0.15f);
                break;
            case ScrollData.ScrollType.Reflection:
                CharacterManager.instance.SetReflectDamage(10);
                break;
            case ScrollData.ScrollType.Escape:
                if (combatData.EnemyType != EnemyData.EnemyType.Boss)
                {
                    Scene.Controller.OnClearScene();
                    StatisticsManager.Instance.CurrentRoom++;
                }
                else return;
                break;
            case ScrollData.ScrollType.Money:
                CharacterManager.instance.GetGold(50);
                break;
            case ScrollData.ScrollType.Grow:
                CharacterManager.selectedCharacter.characterData.IncreaseMaxHp(5);
                break;
            case ScrollData.ScrollType.FirePillar:
                CharacterManager.instance.DamageAllEnemies(20);
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
        else slot1Image.color = Color.white;

        if (slot2Image.sprite == null) slot2Image.color = transparentColor;
        else slot2Image.color = Color.white;

        if (slot3Image.sprite == null) slot3Image.color = transparentColor;
        else slot3Image.color = Color.white;

        slot1ScrollView.scrollType = scrollData.Slot1;
        slot2ScrollView.scrollType = scrollData.Slot2;
        slot3ScrollView.scrollType = scrollData.Slot3;
    }
}
