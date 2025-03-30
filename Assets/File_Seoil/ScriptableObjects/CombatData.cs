using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "Scriptable Objects/CombatData")]
public class CombatData : ScriptableObject
{
    public EnemyData.HabitatType HabitatType;
    public EnemyData.EnemyType EnemyType;

    [SerializeField] private List<TreasureType> treasureData;

    public int CombatSceneCount = 0;

    public void Initialize()
    {
        HabitatType = EnemyData.HabitatType.Forest;
        EnemyType = EnemyData.EnemyType.Common;

        treasureData = new List<TreasureType>();
    }

    private void Awake()
    {
        if(treasureData == null) treasureData = new List<TreasureType>();
    }

    public void AddTreasureData(TreasureType type)
    {
        foreach(TreasureType item in treasureData)
        {
            if (item == type) return;
        }

        treasureData.Add(type);

        if (Scene.treasureShowManager != null) Scene.treasureShowManager.UpdateTreasureImages();
    }

    public void AddAllTreasureData()
    {
        foreach(TreasureType type in Enum.GetValues(typeof(TreasureType)))
        {
            AddTreasureData(type);
        }
    }

    public TreasureType[] TreasureData
    {
        get
        {
            return treasureData.ToArray();
        }
    }

    /// <summary>
    /// UniversalGravitation : �����η�
    /// BusinessAcumen : �������
    /// Condemnation : ����
    /// MoneyBack : �� �ָӴ�
    /// EmergencyFood : ��� �ķ�
    /// GiantResistanceHammer : ����ο� ö��
    /// NobleBlood : ����� ��
    /// BoneCanine : ������ �۰���
    /// GoldAndSilver : �ݰ� ��
    /// GoldenHair : �ݻ� �Ӹ�ī��
    /// WoodPile : ���� ����
    /// CorruptTouch : ������ �ձ�
    /// RingOfTime : �ð��� ��
    /// HolyShield : �ż� ��ȣ��
    /// EtherMemberShip : ���׸� ��ȸ �����
    /// SoulLantern : ��ȥ�� ����
    /// BloodOfResistance : ������ ��
    /// OrderDefenceCloak : �ֹ� ���� ����
    /// CaneOfGravity : �߷��� ������
    /// TalismanOfPower : �����ϴ� ���� Ż������
    /// SwordOfRuinedKing : �ĸ��ϴ� ���� ��
    /// ShoesOfHermes : �츣�޽��� �Ź�
    /// GoldenApple : Ȳ�ݻ��
    /// MultipleCureScroll : ���� ȸ�� ��ũ��
    /// </summary>
    [Serializable]
    public enum TreasureType
    { 
        UniversalGravitation, 
        BusinessAcumen, 
        Condemnation, 
        MoneyBack, 
        EmergencyFood, 
        GiantResistanceHammer,
        NobleBlood,
        BoneCanine,
        GoldAndSilver,
        GoldenHair,
        WoodPile,
        CorruptTouch,
        RingOfTime,
        HolyShield,
        EtherMemberShip,
        SoulLantern,
        TotemOfResistance,
        OrderDefenceCloak,
        CaneOfGravity,
        TalismanOfPower,
        SwordOfRuinedKing,
        ShoesOfHermes,
        GoldenApple,
        MultipleCureScroll
    }
}

public static class TreasureExtension
{
    public static string ToStringByKorean(this CombatData.TreasureType type)
    {
        return type switch
        {
            CombatData.TreasureType.UniversalGravitation => "���� �η�",                // UniversalGravitation : �����η�
            CombatData.TreasureType.BusinessAcumen => "�������",                      // BusinessAcumen : �������
            CombatData.TreasureType.Condemnation => "����",                            // Condemnation : ����
            CombatData.TreasureType.MoneyBack => "�� �ָӴ�",                          // MoneyBack : �� �ָӴ�
            CombatData.TreasureType.EmergencyFood => "��� �ķ�",                      // EmergencyFood : ��� �ķ�
            CombatData.TreasureType.GiantResistanceHammer => "����ο� ö��",          // GiantResistanceHammer : ����ο� ö��
            CombatData.TreasureType.NobleBlood => "����� ��",                         // NobleBlood : ����� ��
            CombatData.TreasureType.BoneCanine => "������ �۰���",                     // BoneCanine : ������ �۰���
            CombatData.TreasureType.GoldAndSilver => "�ݰ� ��",                        // GoldAndSilver : �ݰ� ��
            CombatData.TreasureType.GoldenHair => "�ݻ� �Ӹ�ī��",                     // GoldenHair : �ݻ� �Ӹ�ī��
            CombatData.TreasureType.WoodPile => "���� ����",                           // WoodPile : ���� ����
            CombatData.TreasureType.CorruptTouch => "������ �ձ�",                     // CorruptTouch : ������ �ձ�
            CombatData.TreasureType.RingOfTime => "�ð��� ��",                       // RingOfTime : �ð��� ��
            CombatData.TreasureType.HolyShield => "�ż� ��ȣ��",                       // HolyShield : �ż� ��ȣ��
            CombatData.TreasureType.EtherMemberShip => "���׸� ��ȸ �����",           // EtherMemberShip : ���׸� ��ȸ �����
            CombatData.TreasureType.SoulLantern => "��ȥ�� ����",                      // SoulLantern : ��ȥ�� ����
            CombatData.TreasureType.OrderDefenceCloak => "�ֹ� ���� ����",         // OrderDefenceCloak : �ֹ� ���� ����
            CombatData.TreasureType.CaneOfGravity => "�߷��� ������",                  // CaneOfGravity : �߷��� ������
            CombatData.TreasureType.TalismanOfPower => "�����ϴ� ���� Ż������",      // TalismanOfPower : �����ϴ� ���� Ż������
            CombatData.TreasureType.SwordOfRuinedKing => "�ĸ��ϴ� ���� ��",           // SwordOfRuinedKing : �ĸ��ϴ� ���� ��
            CombatData.TreasureType.ShoesOfHermes => "�츣�޽��� �Ź�",               // ShoesOfHermes : �츣�޽��� �Ź�
            CombatData.TreasureType.GoldenApple => "Ȳ�ݻ��",                         // GoldenApple : Ȳ�ݻ��
            CombatData.TreasureType.MultipleCureScroll => "���� ȸ�� ��ũ��",          // MultipleCureScroll : ���� ȸ�� ��ũ��
            CombatData.TreasureType.TotemOfResistance => "������ ����",

            _ => "�� �� ����",  // �⺻��
        };
    }
}