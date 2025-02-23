using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class CharacterData
{
    [SerializeField]
    private string characterName;
    public string ultimateSkillDescription;
    public GameObject characterPrefab;

    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int currentHp;
    [SerializeField]
    private int maxUltimateGauge;
    [SerializeField]
    private int currentUltimateGauge;
    [SerializeField]
    private int executionRate;

    public Action OnHpChanged;
    public Action OnUltimateGaugeChanged;

    public void Initialize()
    {
        currentUltimateGauge = 0;
    }

    public string CharacterName
    {
        get => characterName;
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = Mathf.Max(value, 1);
            OnHpChanged?.Invoke();
            Debug.Log($"�ִ�ü�� set: {maxHp}");
        }
    }

    public int CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
            OnHpChanged?.Invoke();
            Debug.Log($"����ü�� set: {currentHp}");
        }
    }

    public int MaxUltimateGauge
    {
        get => maxUltimateGauge;
    }

    public int CurrentUltimateGauge
    {
        get => currentUltimateGauge;
        set
        {
            currentUltimateGauge = Mathf.Clamp(value, 0, maxUltimateGauge);
            OnUltimateGaugeChanged?.Invoke();
            Debug.Log($"�ñر������ set: {maxUltimateGauge}");
        }
    }

    public int ExecutionRate
    {
        get => executionRate;
        set
        {
            executionRate = Mathf.Clamp(value, 0, 100);
            OnHpChanged?.Invoke();
            Debug.Log($"ó���� set: {executionRate}");
        }
    }
}