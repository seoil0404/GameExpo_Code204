using UnityEngine;
using System;
using static CombatData;
using static EnemyData;

public class ChestRoom : MonoBehaviour
{
    public CombatData combatData;
    public GameObject level_1;
    public GameObject level_2;
    public GameObject level_3;


    private void Start()
    {
        level_1.SetActive(false);
        level_2.SetActive(false);
        level_3.SetActive(false);

        switch (combatData.HabitatType)
        {
            case HabitatType.Forest:
                level_1.SetActive(true);
                break;
            case HabitatType.Castle:
                level_2.SetActive(true);
                break;
            case HabitatType.DevilCastle:
                level_3.SetActive(true);
                break;
            default:
                Debug.LogWarning("Unknown Habitat Type!");
                break;
        }
    }

    public void GetRelics()
    {
        TreasureType[] availableTreasures = (TreasureType[])Enum.GetValues(typeof(TreasureType));
        if (availableTreasures.Length == 0) return;

        TreasureType randomTreasure = availableTreasures[UnityEngine.Random.Range(0, availableTreasures.Length)];

        combatData.AddTreasureData(randomTreasure);
        Debug.Log($"[LootBox] {randomTreasure} È¹µæ!");
    }

    public void EndChestRoom()
    {
        Scene.Controller.OnClearScene();
        Debug.Log("»óÀÚ¹æ Á¾·á");
    }
}