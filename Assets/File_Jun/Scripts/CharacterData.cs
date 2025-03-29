using UnityEngine;
using System;
using Unity.VisualScripting;

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
    private int baseExecutionRate;
    [SerializeField]
    private int executionRate;
    [SerializeField]
    private int characterATK;
    [SerializeField]
    private int currentCharacterATK;


    public UltimateSkill ultimateSkill;
    public bool NegateNextDamage = false;
    public bool NextAttackLifeSteal = false;

    public Action OnHpChanged;
    public Action OnUltimateGaugeChanged;
	
	public AttackEffectSpawner AttackEffectSpawner;

    public bool IsInvincible = false;
    public void Initialize()
    {
        currentUltimateGauge = 0;
        executionRate = baseExecutionRate;
        currentCharacterATK = characterATK;

        TreasureEffect treasureEffect = GameObject.FindFirstObjectByType<TreasureEffect>();
    }

    public string CharacterName => characterName;

    public int MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = Mathf.Max(value, 1);
            currentHp = Mathf.Min(currentHp, maxHp);
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

    public void ActivateLifeSteal()
    {
        NegateNextDamage = true;
        NextAttackLifeSteal = true;
        Debug.Log("궁극기 발동: 공격 무효화 + 흡혈 활성화");
    }

    public int CharacterATK
    {
        get => characterATK;
        set
        {
            characterATK = Mathf.Max(0, value);
            Debug.Log($"기본 ATK set: {characterATK}");
        }
    }

    public int CurrentCharacterATK
    {
        get => currentCharacterATK;
        set
        {
            currentCharacterATK = Mathf.Max(0, value);
            Debug.Log($"현재 ATK set: {currentCharacterATK}");
        }   
    }

    public void IncreaseMaxHp(int amount)
{
    if (amount <= 0)
    {
        Debug.LogWarning("[CharacterData] 증가시킬 체력 값은 0보다 커야 합니다.");
        return;
    }

    MaxHp += amount;
    Debug.Log($"[{CharacterName}]의 최대 체력이 {amount}만큼 증가하여 {MaxHp}가 되었습니다.");
}

}
