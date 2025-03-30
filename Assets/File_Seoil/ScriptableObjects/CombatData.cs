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

public static class TreasureExtension
{
    public static string ToStringByKorean(this CombatData.TreasureType type)
    {
        return type switch
        {
            CombatData.TreasureType.UniversalGravitation => "만유 인력",                // UniversalGravitation : 만유인력
            CombatData.TreasureType.BusinessAcumen => "사업수완",                      // BusinessAcumen : 사업수완
            CombatData.TreasureType.Condemnation => "단죄",                            // Condemnation : 단죄
            CombatData.TreasureType.MoneyBack => "돈 주머니",                          // MoneyBack : 돈 주머니
            CombatData.TreasureType.EmergencyFood => "비상 식량",                      // EmergencyFood : 비상 식량
            CombatData.TreasureType.GiantResistanceHammer => "대거인용 철퇴",          // GiantResistanceHammer : 대거인용 철퇴
            CombatData.TreasureType.NobleBlood => "고귀한 피",                         // NobleBlood : 고귀한 피
            CombatData.TreasureType.BoneCanine => "관통의 송곳니",                     // BoneCanine : 관통의 송곳니
            CombatData.TreasureType.GoldAndSilver => "금과 은",                        // GoldAndSilver : 금과 은
            CombatData.TreasureType.GoldenHair => "금색 머리카락",                     // GoldenHair : 금색 머리카락
            CombatData.TreasureType.WoodPile => "나무 말뚝",                           // WoodPile : 나무 말뚝
            CombatData.TreasureType.CorruptTouch => "부패한 손길",                     // CorruptTouch : 부패한 손길
            CombatData.TreasureType.RingOfTime => "시간의 고리",                       // RingOfTime : 시간의 고리
            CombatData.TreasureType.HolyShield => "신성 보호막",                       // HolyShield : 신성 보호막
            CombatData.TreasureType.EtherMemberShip => "에테르 상회 멤버십",           // EtherMemberShip : 에테르 상회 멤버십
            CombatData.TreasureType.SoulLantern => "영혼의 랜턴",                      // SoulLantern : 영혼의 랜턴
            CombatData.TreasureType.OrderDefenceCloak => "주문 방어막의 망토",         // OrderDefenceCloak : 주문 방어막의 망토
            CombatData.TreasureType.CaneOfGravity => "중력의 지팡이",                  // CaneOfGravity : 중력의 지팡이
            CombatData.TreasureType.TalismanOfPower => "증폭하는 힘의 탈리스만",      // TalismanOfPower : 증폭하는 힘의 탈리스만
            CombatData.TreasureType.SwordOfRuinedKing => "파멸하는 왕의 검",           // SwordOfRuinedKing : 파멸하는 왕의 검
            CombatData.TreasureType.ShoesOfHermes => "헤르메스의 신발",               // ShoesOfHermes : 헤르메스의 신발
            CombatData.TreasureType.GoldenApple => "황금사과",                         // GoldenApple : 황금사과
            CombatData.TreasureType.MultipleCureScroll => "다중 회복 스크롤",          // MultipleCureScroll : 다중 회복 스크롤
            CombatData.TreasureType.TotemOfResistance => "저항의 토템",

            _ => "알 수 없음",  // 기본값
        };
    }
}