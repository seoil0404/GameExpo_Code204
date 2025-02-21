using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : MonoBehaviour
{
    public EnemyData enemyData;
    public TextMeshProUGUI healthText;

    private EnemySpawner spawner;
    private CharacterManager characterManager;
    private int hp;
    private int atk;
    private float dodgeChance;
    private int comboCount = 0;
    private static List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        characterManager = FindFirstObjectByType<CharacterManager>();

        if (characterManager == null)
        {
            Debug.LogError("CharacterManager�� ã�� �� �����ϴ�!");
        }

        SetStats();
    }

    public int GetHabitatLevel()
    {
        switch (enemyData.habitat)
        {
            case EnemyData.HabitatType.Forest:
                return 1;
            case EnemyData.HabitatType.Castle:
                return 2;
            case EnemyData.HabitatType.DevilCastle:
                return 3;
            default:
                return 0;
        }
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }


    public void SetStats()
    {
        int currentDifficulty = PlayerPrefs.GetInt("Difficulty", 1);
        int habitatLevel = GetHabitatLevel();

        hp = enemyData.baseHP + (habitatLevel * currentDifficulty);
        atk = enemyData.baseATK + (currentDifficulty / Mathf.Max(habitatLevel % 4, 1));
        dodgeChance = enemyData.dodgeChance;

        UpdateHealthText();
    }

    public void PerformTurnAction(Grid grid)
    {
        float actionRoll = Random.Range(0f, 1f);
        if (actionRoll < 0.5f)
        {
            AttackPlayer();
        }
        else
        {
            UseSkill(grid);
        }
    }

    public void AttackPlayer()
    {
        if (characterManager == null)
        {
            Debug.LogError("CharacterManager�� ã�� �� �����ϴ�!");
            return;
        }

        int damage = atk;
        Debug.Log($"[{gameObject.name}]��(��) �÷��̾ �����Ͽ� {damage} �������� �����ϴ�.");
        characterManager.ApplyDamageToCharacter(damage);
    }

    private void UseSkill(Grid grid)
    {
        if (enemyData.enemySkill != null)
        {
            Debug.Log($"[{gameObject.name}]��(��) ��ų�� ����մϴ�!");
            enemyData.enemySkill.ActivateSkill(grid, gameObject);
        }
        else
        {
            Debug.Log($"{gameObject.name}���� �Ҵ�� ��ų�� �����ϴ�.");
        }
    }

    public void ReceiveDamage(int completedLines, int gridColumns)
    {
        float dodgeRoll = Random.Range(0, 100);
        Debug.Log($" ȸ�� üũ: ������({dodgeRoll}) vs ȸ�� Ȯ��({dodgeChance}%)");

        if (dodgeRoll < dodgeChance)
        {
            Debug.Log($" [{gameObject.name}]��(��) ������ ȸ���߽��ϴ�! �������� ���� �ʽ��ϴ�.");
            return;
        }

        int totalBlocksUsed = completedLines * gridColumns;
        int baseDamage = totalBlocksUsed;
        int calculatedDamage = baseDamage + (comboCount * 2);

        hp -= calculatedDamage;

        Debug.Log($" [{gameObject.name}]���� {calculatedDamage} �������� �������ϴ�.");

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }

        comboCount++;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"HP: {hp}";
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name}��(��) �׾����ϴ�!");

        if (Grid.instance != null)
        {
            Grid.instance.RemoveEnemy(gameObject);
        }

        Destroy(gameObject);
    }

    public static void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            Debug.Log($"[EnemyStats] �� �߰���: {enemy.name}, ���� �� ����: {enemies.Count}");
        }
    }

    public static void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"[EnemyStats] �� ���ŵ�: {enemy.name}, ���� �� ����: {enemies.Count}");

            Destroy(enemy);

            if (enemies.Count > 0)
            {
                FindFirstObjectByType<EnemyStats>().StartCoroutine(DelayedSelectRandomEnemy());
            }
            else
            {
                Debug.Log("[EnemyStats] ��� ���� ���ŵ�! CheckIfGameEnded() ����");
                FindFirstObjectByType<Grid>().CheckIfGameEnded();
            }
        }
        else
        {
            Debug.LogWarning($"[EnemyStats] {enemy.name}��(��) ����Ʈ�� ����.");
        }
    }

    private static IEnumerator DelayedSelectRandomEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        EnemySelector.SelectRandomEnemy();
    }

    public static List<GameObject> GetAllEnemies()
    {
        return new List<GameObject>(enemies);
    }

    public int GetAttackDamage()
    {
        return atk;
    }

    public int GetCurrentHp()
    {
        return hp;
    }

    public float GetDodgeChance()
    {
        return dodgeChance;
    }
}
