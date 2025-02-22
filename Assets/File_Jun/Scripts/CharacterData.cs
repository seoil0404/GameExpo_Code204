using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class CharacterData
{
    public string characterName;
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

    public Action OnHpChanged;
    public Action OnUltimateGaugeChanged;

    public void Initialize()
    {
        currentUltimateGauge = 0;
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            currentHp = Mathf.Max(value, 1);
            OnHpChanged?.Invoke();  
            Debug.Log($"최대체력 set: {maxHp}");
        }
    }

    public int CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
            OnHpChanged?.Invoke();
            Debug.Log($"현재체력 set: {currentHp}");
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
            Debug.Log($"궁극기게이지 set: {maxUltimateGauge}");
        }
    }
}