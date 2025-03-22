using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HabitatBackground
{
    public EnemyData.HabitatType habitatType;
    public GameObject background;
}

public class BackgroundManager : MonoBehaviour
{
    public List<HabitatBackground> habitatBackgrounds = new List<HabitatBackground>();
    public GameObject defaultBackground;
    public CombatData combatData;

    private Dictionary<EnemyData.HabitatType, GameObject> backgroundDict = new Dictionary<EnemyData.HabitatType, GameObject>();

    private void Awake()
    {
        if (combatData == null)
        {
            Debug.LogError("[BackgroundManager] CombatData가 Inspector에서 설정되지 않았습니다!");
            return;
        }

        Debug.Log($"[BackgroundManager] 감지된 HabitatType: {combatData.HabitatType}");

        InitializeBackgrounds();
        SetBackground(combatData.HabitatType);
    }

    private void InitializeBackgrounds()
    {
        backgroundDict.Clear();

        foreach (var entry in habitatBackgrounds)
        {
            if (!backgroundDict.ContainsKey(entry.habitatType))
            {
                backgroundDict.Add(entry.habitatType, entry.background);
            }
        }

        foreach (var bg in backgroundDict.Values)
        {
            bg.SetActive(false);
        }
    }

    public void SetBackground(EnemyData.HabitatType habitatType)
    {
        foreach (var bg in backgroundDict.Values)
        {
            bg.SetActive(false);
        }

        if (backgroundDict.ContainsKey(habitatType))
        {
            backgroundDict[habitatType].SetActive(true);
            Debug.Log($"[BackgroundManager] 배경 변경됨: {habitatType}");
        }
        else if (defaultBackground != null)
        {
            defaultBackground.SetActive(true);
            Debug.LogWarning("[BackgroundManager] 해당 서식지에 맞는 배경이 없어 기본 배경을 사용합니다.");
        }
        else
        {
            Debug.LogError("[BackgroundManager] 배경 설정이 없습니다!");
        }
    }
}
