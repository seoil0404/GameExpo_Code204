using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScrollData", menuName = "Scriptable Objects/ScrollData")]
public class ScrollData : ScriptableObject
{
    [Header("Slots")]
    public ScrollType Slot1;
    public ScrollType Slot2;
    public ScrollType Slot3;

    public void Initialize()
    {
        Slot1 = ScrollType.None;
        Slot2 = ScrollType.None;
        Slot3 = ScrollType.None;
    }

    #region Scroll
    /// <summary>
    /// ReStart ������� ��ũ��,
    /// Delete ������ ��ũ��,
    /// Speed �ż��� ��ũ��,
    /// FireBall ȭ������ ��ũ��,
    /// Curse ������ ��ũ��,
    /// Strengh ��ȭ�� ��ũ��,
    /// Energy ����� ��ũ��,
    /// Poision �ߵ��� ��ũ��,
    /// Heal ġ���� �� ��ũ��,
    /// Reflection �ݻ��� ��ũ��,
    /// Escape ������ ��ũ��,
    /// Money ���� ��ũ��,
    /// Grow �޼����� ��ũ��,
    /// FirePillar ȭ�� ����� ��ũ��,
    /// Fill �̿ϼ� ��ũ��,
    /// Life ��Ȱ�� ��ũ��
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

    public enum ScrollRarity
    {
        Normal,
        Rare,
        Epic,
        Legendary
    }

    private static readonly ScrollType[] normalTypes = {
        ScrollType.ReStart, ScrollType.Delete, ScrollType.Speed, ScrollType.FireBall, ScrollType.Curse, ScrollType.Strengh, ScrollType.Energy, ScrollType.Poision, ScrollType.Heal
    };

    private static readonly ScrollType[] rareTypes = {
        ScrollType.Reflection, ScrollType.Escape, ScrollType.Money, ScrollType.Grow
    };

    private static readonly ScrollType[] epicTypes = {
        ScrollType.FirePillar, ScrollType.Fill
    };

    private static readonly ScrollType[] legendaryTypes = {
        ScrollType.Life
    };

    public static ScrollType[] NormalTypes => normalTypes.ToArray();
    public static ScrollType[] RareTypes => rareTypes.ToArray();
    public static ScrollType[] EpicTypes => epicTypes.ToArray();
    public static ScrollType[] LegendaryTypes => legendaryTypes.ToArray();

    public static ScrollType[] GetScrollTypesByRarity(ScrollRarity scrollRarity)
    {
        switch (scrollRarity)
        {
            case ScrollRarity.Normal:
                return NormalTypes;
            case ScrollRarity.Rare:
                return RareTypes;
            case ScrollRarity.Epic:
                return EpicTypes;
            case ScrollRarity.Legendary:
                return LegendaryTypes;
        }

#if UNITY_EDITOR
        throw new System.Exception("Unknown ScrollRarity : " + scrollRarity.ToString());
#else
        return NormalTypes;
#endif
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

    public static ScrollRarity GetRarity(ScrollType type)
    {
        switch (type)
        {
            case ScrollType.ReStart:
                return ScrollRarity.Normal;
            case ScrollType.Delete:
                return ScrollRarity.Normal;
            case ScrollType.Speed:
                return ScrollRarity.Normal;
            case ScrollType.FireBall:
                return ScrollRarity.Normal;
            case ScrollType.Curse:
                return ScrollRarity.Normal;
            case ScrollType.Strengh:
                return ScrollRarity.Normal;
            case ScrollType.Energy:
                return ScrollRarity.Normal;
            case ScrollType.Poision:
                return ScrollRarity.Normal;
            case ScrollType.Heal:
                return ScrollRarity.Normal;
            case ScrollType.Reflection:
                return ScrollRarity.Rare;
            case ScrollType.Escape:
                return ScrollRarity.Rare;
            case ScrollType.Money:
                return ScrollRarity.Rare;
            case ScrollType.Grow:
                return ScrollRarity.Rare;
            case ScrollType.FirePillar:
                return ScrollRarity.Epic;
            case ScrollType.Fill:
                return ScrollRarity.Epic;
            case ScrollType.Life:
                return ScrollRarity.Legendary;
            default:
                throw new System.Exception("Unknown ScrollType : " + type.ToString());
        }
    }

    public static string GetDescription(ScrollType type)
    {
        switch (type)
        {
            case ScrollType.None:
                return "����\r\n�� �����̴�. ��ũ���� ������ �� �ִ�.";
            case ScrollType.ReStart:
                return "������� ��ũ��\n\n�ɷ�\r\n��ο� �� �̳��� �ϳ��� �ٽ� �̴´�.";
            case ScrollType.Delete:
                return "������ ��ũ��\n\n�ɷ�\r\nȦ�� �ȿ� �̳븦 �����Ѵ�.";
            case ScrollType.Speed:
                return "�ż��� ��ũ��\n\n�ɷ�\r\n1�� ���� ȸ���� 50%�� �ȴ�.";
            case ScrollType.FireBall:
                return "ȭ������ ��ũ��\n\n�ɷ�\r\n������ ��� �ϳ����� 10�� �������� ������.";
            case ScrollType.Curse:
                return "������ ��ũ��\n\n�ɷ�\r\n������ ��� �ϳ����� ġ�� ���� 3�� �ο��Ѵ�.";
            case ScrollType.Strengh:
                return "��ȭ�� ��ũ��\n\n�ɷ�\r\n��ȭ ������� �����Ѵ�.";
            case ScrollType.Energy:
                return "����� ��ũ��\n\n�ɷ�\r\n1�� ���� ���� 3����Ѵ�.";
            case ScrollType.Poision:
                return "�ߵ��� ��ũ��\n\n�ɷ�\r\n������ ��� �ϳ����� �� 5�� �ο��Ѵ�.";
            case ScrollType.Heal:
                return "ġ���� �� ��ũ��\n\n�ɷ�\r\n�÷��̾� �ִ� ü���� 15%�� ȸ���Ѵ�.";
            case ScrollType.Reflection:
                return "�ݻ��� ��ũ��\n\n�ɷ�\r\n�÷��̾�� ���� 10�� �ο��Ѵ�.";
            case ScrollType.Escape:
                return "������ ��ũ��\n\n�ɷ�\r\n������ �����ϰ� ����ģ��. (���� ����)";
            case ScrollType.Money:
                return "���� ��ũ��\n\n�ɷ�\r\n�� 50�� ȹ���Ѵ�.";
            case ScrollType.Grow:
                return "�޼����� ��ũ��\n\n�ɷ�\r\n�÷��̾��� �ִ� ü���� 5 �����Ѵ�.";
            case ScrollType.FirePillar:
                return "ȭ������� ��ũ��\n\n�ɷ�\r\n��� ��ο��� 20 �������� ������.";
            case ScrollType.Fill:
                return "�̿ϼ� ��ũ��\n\n�ɷ�\r\n�� ��ũ�� ������ �������� ä���. (�ߺ�X)";
            case ScrollType.Life:
                return "��Ȱ�� ��ũ��\n\n�ɷ�\r\n���� �ִ� ü���� 10�����Ѵ�.\n�÷��̾ ����� �� �ڵ����� ���Ǿ� �ִ� ü���� 30%�� ȸ���Ѵ�.";
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
