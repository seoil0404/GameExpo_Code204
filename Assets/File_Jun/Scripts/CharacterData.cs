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
    public bool NextAttackLifeSteal = false;

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

    public void ApplyCondemnationEffect()
    {
        if (TreasureEffect.IsTreasureActive(TreasureEffect.TreasureType.Condemnation))
        {
            ExecutionRate += 5;
            Debug.Log($"{CharacterName}의 처형율이 5 증가하여 현재 {ExecutionRate}이(가) 되었습니다.");
        }
    }


    public void ActivateLifeSteal()
    {
        NegateNextDamage = true; // 적 공격 무효화 1회
        NextAttackLifeSteal = true; // 흡혈 1회 발동
        Debug.Log("궁극기 발동: 공격 무효화 + 흡혈 활성화");
    }

}
