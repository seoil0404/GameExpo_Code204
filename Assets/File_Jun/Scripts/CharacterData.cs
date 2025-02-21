using UnityEngine;
using System;

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public int ultimateGaugeMax;
    public string ultimateSkillDescription;
    public GameObject characterPrefab;

    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int currentHp;
    [SerializeField]
    private int currentUltimateGauge;

    public Action OnHpChanged;

    public void Initialize()
    {
        currentUltimateGauge = 0;
    }

    public int MaxHp
    {
        get { return maxHp;}
        set
        {
            currentHp = Mathf.Max(value, 1);
            OnHpChanged?.Invoke();
            Debug.Log($"최대체력 set: {maxHp}");
        }
    }

    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
            OnHpChanged?.Invoke();
            Debug.Log($"현재체력 set: {currentHp}");
        }
    }

    public int CurrentUltimateGauge
    {
        get { return currentUltimateGauge; }
        set { currentUltimateGauge = Mathf.Max(value, 0); }
    }
}