using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "Scriptable Objects/CombatData")]
public class CombatData : ScriptableObject
{
    public EnemyData.HabitatType HabitatType;
    public EnemyData.EnemyType EnemyType;

    [SerializeField] private List<TreasureType> treasureData;

    public void Initialize()
    {
        HabitatType = EnemyData.HabitatType.Forest;
        EnemyType = EnemyData.EnemyType.Common;

        Debug.Log("Clear");
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

        Scene.treasureShowManager.UpdateTreasureImages();
    }

    public void AddAllTreasureData()
    {
        foreach(TreasureType type in Enum.GetValues(typeof(TreasureType))) 
            AddTreasureData(type);
    }

    public TreasureType[] TreasureData
    {
        get
        {
            return treasureData.ToArray();
        }
    }

    /// <summary>
    /// UniversalGravitation : 만유인력
    /// BusinessAcumen : 사업수완
    /// Condemnation : 단죄
    /// MoneyBack : 돈 주머니
    /// EmergencyFood : 비상 식량
    /// GiantResistanceHammer : 대거인용 철퇴
    /// NobleBlood : 고귀한 피
    /// BoneCanine : 관통의 송곳니
    /// GoldAndSilver : 금과 은
    /// GoldenHair : 금색 머리카락
    /// WoodPile : 나무 말뚝
    /// CorruptTouch : 부패한 손길
    /// RingOfTime : 시간의 고리
    /// HolyShield : 신성 보호막
    /// EtherMemberShip : 에테르 상회 멤버십
    /// SoulLantern : 영혼의 랜턴
    /// BloodOfResistance : 저항의 피
    /// OrderDefenceCloak : 주문 방어막의 망토
    /// CaneOfGravity : 중력의 지팡이
    /// TalismanOfPower : 증폭하는 힘의 탈리스만
    /// SwordOfRuinedKing : 파멸하는 왕의 검
    /// ShoesOfHermes : 헤르메스의 신발
    /// GoldenApple : 황금사과
    /// MultipleCureScroll : 다중 회복 스크롤
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