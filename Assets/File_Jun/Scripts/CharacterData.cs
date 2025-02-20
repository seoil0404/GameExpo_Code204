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

    public event Action OnHpChanged;

    public void Initialize()
    {
        currentHp = maxHp;
        currentUltimateGauge = 0;
    }

    public int MaxHp
    {
        get { return maxHp; }
        set
        {
            currentHp = Mathf.Max(value, 1);
            OnHpChanged?.Invoke();
        }
    }

    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
            OnHpChanged?.Invoke();
        }
    }

    public int CurrentUltimateGauge
    {
        get { return currentUltimateGauge; }
        set { currentUltimateGauge = Mathf.Max(value, 0); }
    }
}