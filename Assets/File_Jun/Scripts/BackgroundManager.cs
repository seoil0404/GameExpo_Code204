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
            Debug.LogError("[BackgroundManager] CombatData�� Inspector���� �������� �ʾҽ��ϴ�!");
            return;
        }

        Debug.Log($"[BackgroundManager] ������ HabitatType: {combatData.HabitatType}");

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
            Debug.Log($"[BackgroundManager] ��� �����: {habitatType}");
        }
        else if (defaultBackground != null)
        {
            defaultBackground.SetActive(true);
            Debug.LogWarning("[BackgroundManager] �ش� �������� �´� ����� ���� �⺻ ����� ����մϴ�.");
        }
        else
        {
            Debug.LogError("[BackgroundManager] ��� ������ �����ϴ�!");
        }
    }
}
