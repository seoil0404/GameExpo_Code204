using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : MonoBehaviour
{
    public EnemyData enemyData;
    public TextMeshProUGUI healthText;

    private EnemySpawner spawner;
    private int hp;
    private int atk;
    private float dodgeChance;
    private int comboCount = 0;

    private static List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        spawner = FindFirstObjectByType<EnemySpawner>();

        if (spawner == null)
        {
            Debug.LogError("EnemySpawner를 찾을 수 없습니다!");
        }
    }

    public void SetStats(int difficulty)
    {
        hp = enemyData.baseHP + (5 * difficulty);
        atk = enemyData.baseATK + (5 / Mathf.Max(difficulty % 4, 1));
        dodgeChance = enemyData.dodgeChance;
        UpdateHealthText();
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    public void PerformAction(Grid grid)
    {
        if (Random.Range(0, 2) == 0)
        {
            int completedLines = 1;
            int gridColumns = 8;
            ReceiveDamage(completedLines, gridColumns);
        }
        else
        {
            UseSkill(grid);
        }
    }

    private void UseSkill(Grid grid)
    {
        if (enemyData.enemySkill != null)
        {
            enemyData.enemySkill.ActivateSkill(grid, gameObject);
        }
        else
        {
            Debug.Log($"{gameObject.name}에게 할당된 스킬이 없습니다.");
        }
    }

    public void ReceiveDamage(int completedLines, int gridColumns)
    {
        float dodgeRoll = Random.Range(0, 100);
        Debug.Log($" 회피 체크: 랜덤값({dodgeRoll}) vs 회피 확률({dodgeChance}%)");

        if (dodgeRoll < dodgeChance)
        {
            Debug.Log($" [{gameObject.name}]이(가) 공격을 회피했습니다! 데미지를 받지 않습니다.");
            return;
        }

        int totalBlocksUsed = completedLines * gridColumns;
        int baseDamage = totalBlocksUsed;
        int calculatedDamage = baseDamage + (comboCount * 2);

        hp -= calculatedDamage;

        Debug.Log($" [{gameObject.name}]에게 {calculatedDamage} 데미지를 입혔습니다.");

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
        Debug.Log($"{gameObject.name}이(가) 죽었습니다!");

        RemoveEnemy(gameObject);
    }

    public static void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            Debug.Log($"[EnemyStats] 적 추가됨: {enemy.name}, 현재 적 개수: {enemies.Count}");
        }
    }

    public static void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"[EnemyStats] 적 제거됨: {enemy.name}, 남은 적 개수: {enemies.Count}");

            Destroy(enemy);

            if (enemies.Count > 0)
            {
                FindFirstObjectByType<EnemyStats>().StartCoroutine(DelayedSelectRandomEnemy());
            }
            else
            {
                Debug.Log("[EnemyStats] 모든 적이 제거됨! CheckIfGameEnded() 실행");
                FindFirstObjectByType<Grid>().CheckIfGameEnded();
            }
        }
        else
        {
            Debug.LogWarning($"[EnemyStats] {enemy.name}이(가) 리스트에 없음.");
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
