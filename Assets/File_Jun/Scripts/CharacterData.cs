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

    // �� �� ĳ������ �ñر� ��ų�� ���� ���� ��ȿȭ ����
    public UltimateSkill ultimateSkill;
    public bool NegateNextDamage = false;

    public Action OnHpChanged;
    public Action OnUltimateGaugeChanged;

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
            currentHp = Mathf.Max(value, 1);
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

    // ���� �̻� ȿ���� ��� ġ���ϴ� �޼���
    public void CureStatusEffects()
    {
        Debug.Log($"{CharacterName}�� ���� �̻� ȿ���� ��� ġ��Ǿ����ϴ�.");
        // ���� ���ӿ����� �����̻� ����� �ʱ�ȭ�ϴ� ������ ��������.
    }

    // ���� ȿ��: ��밡 ���� ������ ���ݸ�ŭ ȸ���մϴ�.
    public void ActivateLifeSteal(int damageDealt)
    {
        int healAmount = damageDealt / 2;
        CurrentHp += healAmount;
        Debug.Log($"{CharacterName}�� ���� ȿ�� �ߵ�: {healAmount} ��ŭ ü���� ȸ���Ǿ����ϴ�. (������ ����)");
    }
}
