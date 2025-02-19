using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemyStats : MonoBehaviour
{
    public EnemyData enemyData;
    public TextMeshProUGUI healthText;

    private EnemySpawner spawner;
    private int hp;
    private int atk;
    private float dodgeChance;

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
            Attack(grid);
        }
        else
        {
            UseSkill(grid);
        }
    }

    private void Attack(Grid grid)
    {
        GameObject target = grid.GetSelectedEnemy();
        if (target == null)
        {
            Debug.Log("공격할 적이 없습니다.");
            return;
        }

        var enemyStats = target.GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("적의 EnemyStats를 찾을 수 없습니다.");
            return;
        }

        float dodgeChance = enemyStats.GetDodgeChance();
        int dodgeRoll = Random.Range(0, 100);

        Debug.Log($"회피 체크: 랜덤값({dodgeRoll}) vs 회피 확률({dodgeChance}%)");

        if (dodgeRoll < dodgeChance)
        {
            Debug.Log($"{target.name}이(가) 공격을 회피했습니다! 데미지를 받지 않습니다.");
            return;
        }

        int damage = enemyData.baseATK;
        enemyStats.TakeDamage(damage);
        Debug.Log($"{target.name}에게 {damage}의 데미지를 입혔습니다.");
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

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }

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

        
        if (spawner != null)
        {
            spawner.RemoveEnemy(gameObject);
        }
        else
        {
            Debug.LogError("EnemySpawner를 찾을 수 없어 적을 리스트에서 제거하지 못했습니다!");
        }

        Destroy(gameObject);
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
