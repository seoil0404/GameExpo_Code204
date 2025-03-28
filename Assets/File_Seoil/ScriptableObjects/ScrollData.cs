using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScrollData", menuName = "Scriptable Objects/ScrollData")]
public class ScrollData : ScriptableObject
{
    [Header("Slots")]
    public ScrollType Slot1;
    public ScrollType Slot2;
    public ScrollType Slot3;

    #region Scroll
    /// <summary>
    /// ReStart 재시작의 스크롤,
    /// Delete 제거의 스크롤,
    /// Speed 신속의 스크롤,
    /// FireBall 화염구의 스크롤,
    /// Curse 저주의 스크롤,
    /// Strengh 강화의 스크롤,
    /// Energy 기운의 스크롤,
    /// Poision 중독의 스크롤,
    /// Heal 치유의 빛 스크롤,
    /// Reflection 반사의 스크롤,
    /// Escape 도망의 스크롤,
    /// Money 돈의 스크롤,
    /// Grow 급성장의 스크롤,
    /// FirePillar 화염 기둥의 스크롤,
    /// Fill 미완성 스크롤,
    /// Life 부활의 스크롤
    /// </summary>
    public enum ScrollType
    {
        None,
        ReStart,
        Delete,
        Speed,
        FireBall,
        Curse,
        Strengh,
        Energy,
        Poision,
        Heal,
        Reflection,
        Escape,
        Money,
        Grow,
        FirePillar,
        Fill,
        Life
    }
    #endregion

    [Header("Sprites")]
    [SerializeField] private Sprite ReStart;
    [SerializeField] private Sprite Delete;
    [SerializeField] private Sprite Speed;
    [SerializeField] private Sprite FireBall;
    [SerializeField] private Sprite Curse;
    [SerializeField] private Sprite Strengh;
    [SerializeField] private Sprite Energy;
    [SerializeField] private Sprite Poision;
    [SerializeField] private Sprite Heal;
    [SerializeField] private Sprite Reflection;
    [SerializeField] private Sprite Escape;
    [SerializeField] private Sprite Money;
    [SerializeField] private Sprite Grow;
    [SerializeField] private Sprite FirePillar;
    [SerializeField] private Sprite Fill;
    [SerializeField] private Sprite Life;

    public static string GetDescription(ScrollType type)
    {
        switch(type)
        {
            case ScrollType.None:
                return "설명\r\n빈 슬롯이다. 스크롤을 보관할 수 있다.";
            case ScrollType.ReStart:
                return "재시작의 스크롤\n\n능력\r\n드로우 된 미노중 하나만 다시 뽑는다.";
            case ScrollType.Delete:
                return "제거의 스크롤\n\n능력\r\n홀드 안에 미노를 제거한다.";
            case ScrollType.Speed:
                return "신속의 스크롤\n\n능력\r\n1턴 동안 회피율 50%가 된다.";
            case ScrollType.FireBall:
                return "화염구의 스크롤\n\n능력\r\n지정한 상대 하나에게 10의 데미지를 입힌다.";
            case ScrollType.Curse:
                return "저주의 스크롤\n\n능력\r\n지정한 상대 하나에게 치유 감소 3을 부여한다.";
            case ScrollType.Strengh:
                return "강화의 스크롤\n\n능력\r\n약화 디버프를 해제한다.";
            case ScrollType.Energy:
                return "기운의 스크롤\n\n능력\r\n1턴 동안 힘이 3상승한다.";
            case ScrollType.Poision:
                return "중독의 스크롤\n\n능력\r\n지정한 상대 하나에게 독 5를 부여한다.";
            case ScrollType.Heal:
                return "치유의 빛 스크롤\n\n능력\r\n플레이어 최대 체력의 15%를 회복한다.";
            case ScrollType.Reflection:
                return "반사의 스크롤\n\n능력\r\n플레이어에게 가시 10을 부여한다.";
            case ScrollType.Escape:
                return "도망의 스크롤\n\n능력\r\n보상을 포기하고 도망친다. (보스 제외)";
            case ScrollType.Money:
                return "돈의 스크롤\n\n능력\r\n돈 50을 획득한다.";
            case ScrollType.Grow:
                return "급성장의 스크롤\n\n능력\r\n플레이어의 최대 체력이 5 증가한다.";
            case ScrollType.FirePillar:
                return "화염기둥의 스크롤\n\n능력\r\n상대 모두에게 20 데미지를 입힌다.";
            case ScrollType.Fill:
                return "미완성 스크롤\n\n능력\r\n빈 스크롤 슬롯을 무작위로 채운다. (중복X)";
            case ScrollType.Life:
                return "부활의 스크롤\n\n능력\r\n사용시 최대 체력이 10증가한다.\n플레이어가 사망할 때 자동으로 사용되어 최대 체력의 30%를 회복한다.";
            default:
                return null;
        }
    }

    public Sprite GetImage(ScrollType type)
    {
        switch (type)
        {
            case ScrollType.None:
                return null;
            case ScrollType.ReStart:
                return ReStart;
            case ScrollType.Delete:
                return Delete;
            case ScrollType.Speed:
                return Speed;
            case ScrollType.FireBall:
                return FireBall;
            case ScrollType.Curse:
                return Curse;
            case ScrollType.Strengh:
                return Strengh;
            case ScrollType.Energy:
                return Energy;
            case ScrollType.Poision:
                return Poision;
            case ScrollType.Heal:
                return Heal;
            case ScrollType.Reflection:
                return Reflection;
            case ScrollType.Escape:
                return Escape;
            case ScrollType.Money:
                return Money;
            case ScrollType.Grow:
                return Grow;
            case ScrollType.FirePillar:
                return FirePillar;
            case ScrollType.Fill:
                return Fill;
            case ScrollType.Life:
                return Life;
            default:
                return null;
        }
    }
}
