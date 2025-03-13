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

    public void ApplyCondemnationEffect()
    {
        if (TreasureEffect.IsTreasureActive(TreasureEffect.TreasureType.Condemnation))
        {
            ExecutionRate += 5;
            Debug.Log($"{CharacterName}�� ó������ 5 �����Ͽ� ���� {ExecutionRate}��(��) �Ǿ����ϴ�.");
        }
    }


    public void ActivateLifeSteal()
    {
        NegateNextDamage = true; // �� ���� ��ȿȭ 1ȸ
        NextAttackLifeSteal = true; // ���� 1ȸ �ߵ�
        Debug.Log("�ñر� �ߵ�: ���� ��ȿȭ + ���� Ȱ��ȭ");
    }

}
