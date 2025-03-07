using UnityEngine;
using System;

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

    
    public UltimateSkill ultimateSkill;
    public bool NegateNextDamage = false;

    public Action OnHpChanged;
    public Action OnUltimateGaugeChanged;
	
	public AttackEffectSpawner AttackEffectSpawner;

    public void Initialize()
    {
        currentUltimateGauge = 0;
    }

    public string CharacterName => characterName;

    public int MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = Mathf.Max(value, 1);
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

	public bool IsDead =>
		currentHp <= 0;

    public int MaxUltimateGauge => maxUltimateGauge;

    public int CurrentUltimateGauge
    {
        get => currentUltimateGauge;
        set
        {
            currentUltimateGauge = Mathf.Clamp(value, 0, maxUltimateGauge);
            OnUltimateGaugeChanged?.Invoke();
            Debug.Log($"궁극기게이지 set: {currentUltimateGauge}");
        }
    }

    public int ExecutionRate
    {
        get => executionRate;
        set
        {
            executionRate = Mathf.Clamp(value, 0, 100);
            OnHpChanged?.Invoke();
            Debug.Log($"처형율 set: {executionRate}");
        }
    }

    
    public void CureStatusEffects()
    {
        Debug.Log($"{CharacterName}의 상태 이상 효과가 모두 치료되었습니다.");
        
    }

   
    public void ActivateLifeSteal(int damageDealt)
    {
        int healAmount = damageDealt / 2;
        CurrentHp += healAmount;
        Debug.Log($"{CharacterName}의 흡혈 효과 발동: {healAmount} 만큼 체력이 회복되었습니다. (피해의 절반)");
    }
}
