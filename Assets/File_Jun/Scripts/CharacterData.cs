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
            Debug.Log($"�ñر������ set: {currentUltimateGauge}");
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
    
    public void CureStatusEffects()
    {
        Debug.Log($"{CharacterName}�� ���� �̻� ȿ���� ��� ġ��Ǿ����ϴ�.");
        
    }

    public void ActivateLifeSteal()
    {
        NegateNextDamage = true;
        NextAttackLifeSteal = true;
        Debug.Log("�ñر� �ߵ�: ���� ��ȿȭ + ���� Ȱ��ȭ");
    }

    public int CharacterATK
    {
        get => characterATK;
        set
        {
            characterATK = Mathf.Max(0, value);
            Debug.Log($"�⺻ ATK set: {characterATK}");
        }
    }

    public int CurrentCharacterATK
    {
        get => currentCharacterATK;
        set
        {
            currentCharacterATK = Mathf.Max(0, value);
            Debug.Log($"���� ATK set: {currentCharacterATK}");
        }   
    }

    public void IncreaseMaxHp(int amount)
{
    if (amount <= 0)
    {
        Debug.LogWarning("[CharacterData] ������ų ü�� ���� 0���� Ŀ�� �մϴ�.");
        return;
    }

    MaxHp += amount;
    Debug.Log($"[{CharacterName}]�� �ִ� ü���� {amount}��ŭ �����Ͽ� {MaxHp}�� �Ǿ����ϴ�.");
}

}
