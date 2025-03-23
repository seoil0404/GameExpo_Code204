using System.Collections.Generic;
using UnityEngine;

public class TreasureEffect : MonoBehaviour
{
    [Header("보물 데이터를 가져올 CombatData")]
    public CombatData combatData;

    [Header("보물 활성화 상태")]
    public bool UniversalGravitation;
    public bool BusinessAcumen;
    public bool Condemnation;
    public bool MoneyBack;
    public bool EmergencyFood;
    public bool GiantResistanceHammer;
    public bool NobleBlood;
    public bool BoneCanine;
    public bool GoldAndSilver;
    public bool GoldenHair;
    public bool WoodPile;
    public bool CorruptTouch;
    public bool RingOfTime;
    public bool HolyShield;
    public bool EtherMemberShip;
    public bool SoulLantern;
    public bool TotemOfResistance;
    public bool OrderDefenceCloak;
    public bool CaneOfGravity;
    public bool TalismanOfPower;
    public bool SwordOfRuinedKing;
    public bool ShoesOfHermes;
    public bool GoldenApple;
    public bool MultipleCureScroll;

    void Awake()
    {
        if (combatData == null)
        {
            Debug.LogWarning("[TreasureEffect] CombatData가 연결되지 않았습니다.");
            return;
        }

        foreach (var treasure in combatData.TreasureData)
        {
            switch (treasure)
            {
                case CombatData.TreasureType.UniversalGravitation: UniversalGravitation = true; break;
                case CombatData.TreasureType.BusinessAcumen: BusinessAcumen = true; break;
                case CombatData.TreasureType.Condemnation: Condemnation = true; break;
                case CombatData.TreasureType.MoneyBack: MoneyBack = true; break;
                case CombatData.TreasureType.EmergencyFood: EmergencyFood = true; break;
                case CombatData.TreasureType.GiantResistanceHammer: GiantResistanceHammer = true; break;
                case CombatData.TreasureType.NobleBlood: NobleBlood = true; break;
                case CombatData.TreasureType.BoneCanine: BoneCanine = true; break;
                case CombatData.TreasureType.GoldAndSilver: GoldAndSilver = true; break;
                case CombatData.TreasureType.GoldenHair: GoldenHair = true; break;
                case CombatData.TreasureType.WoodPile: WoodPile = true; break;
                case CombatData.TreasureType.CorruptTouch: CorruptTouch = true; break;
                case CombatData.TreasureType.RingOfTime: RingOfTime = true; break;
                case CombatData.TreasureType.HolyShield: HolyShield = true; break;
                case CombatData.TreasureType.EtherMemberShip: EtherMemberShip = true; break;
                case CombatData.TreasureType.SoulLantern: SoulLantern = true; break;
                case CombatData.TreasureType.TotemOfResistance: TotemOfResistance = true; break;
                case CombatData.TreasureType.OrderDefenceCloak: OrderDefenceCloak = true; break;
                case CombatData.TreasureType.CaneOfGravity: CaneOfGravity = true; break;
                case CombatData.TreasureType.TalismanOfPower: TalismanOfPower = true; break;
                case CombatData.TreasureType.SwordOfRuinedKing: SwordOfRuinedKing = true; break;
                case CombatData.TreasureType.ShoesOfHermes: ShoesOfHermes = true; break;
                case CombatData.TreasureType.GoldenApple: GoldenApple = true; break;
                case CombatData.TreasureType.MultipleCureScroll: MultipleCureScroll = true; break;
            }
        }

        Debug.Log("[TreasureEffect] 보물 활성화 완료");
    }
}
