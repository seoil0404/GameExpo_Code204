using UnityEngine;
using System;
using static CombatData;

public class ChestRoom : MonoBehaviour
{
    public CombatData combatData;

    public void GetRelics()
    {
        TreasureType[] availableTreasures = (TreasureType[])Enum.GetValues(typeof(TreasureType));
        if (availableTreasures.Length == 0) return;

        TreasureType randomTreasure = availableTreasures[UnityEngine.Random.Range(0, availableTreasures.Length)];

        combatData.AddTreasureData(randomTreasure);
        Debug.Log($"[LootBox] {randomTreasure} ȹ��!");
    }

    public void EndChestRoom()
    {
        Scene.Controller.OnClearScene();
        Debug.Log("���ڹ� ����");
    }
}