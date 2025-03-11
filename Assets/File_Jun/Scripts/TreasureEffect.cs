using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreasureEffect", menuName = "Treasure/TreasureEffect")]
public class TreasureEffect : ScriptableObject
{
    public enum TreasureType
    {
        UniversalGravitation,
        BusinessAcumen,
        Condemnation,
        MoneyBack,
        EmergencyFood,
        GiantResistanceHammer
    }

    public TreasureType treasureType;
    public bool isActive = false;

    private static List<TreasureEffect> activeTreasures = new List<TreasureEffect>();

    public static bool IsTreasureActive(TreasureType type)
    {
        foreach (var treasure in activeTreasures)
        {
            if (treasure.treasureType == type && treasure.isActive)
            {
                return true;
            }
        }
        return false;
    }

    public void ActivateTreasure()
    {
        if (!activeTreasures.Contains(this))
        {
            activeTreasures.Add(this);
            isActive = true;
            Debug.Log($"{treasureType} 보물이 활성화되었습니다!");
        }
    }

    public static bool IsUniversalGravitationActive() => IsTreasureActive(TreasureType.UniversalGravitation);
    public static bool IsBusinessAcumenActive() => IsTreasureActive(TreasureType.BusinessAcumen);
    public static bool IsCondemnationActive() => IsTreasureActive(TreasureType.Condemnation);
    public static bool IsMoneyBackActive() => IsTreasureActive(TreasureType.MoneyBack);
    public static bool IsEmergencyFoodActive() => IsTreasureActive(TreasureType.EmergencyFood);
    public static bool IsGiantResistanceHammerActive() => IsTreasureActive(TreasureType.GiantResistanceHammer);
}
