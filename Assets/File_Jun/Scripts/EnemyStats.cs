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
            Debug.LogError("EnemySpawner�� ã�� �� �����ϴ�!");
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
            Debug.Log("������ ���� �����ϴ�.");
            return;
        }

        var enemyStats = target.GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("���� EnemyStats�� ã�� �� �����ϴ�.");
            return;
        }

        float dodgeChance = enemyStats.GetDodgeChance();
        int dodgeRoll = Random.Range(0, 100);

        Debug.Log($"ȸ�� üũ: ������({dodgeRoll}) vs ȸ�� Ȯ��({dodgeChance}%)");

        if (dodgeRoll < dodgeChance)
        {
            Debug.Log($"{target.name}��(��) ������ ȸ���߽��ϴ�! �������� ���� �ʽ��ϴ�.");
            return;
        }

        int damage = enemyData.baseATK;
        enemyStats.TakeDamage(damage);
        Debug.Log($"{target.name}���� {damage}�� �������� �������ϴ�.");
    }

    private void UseSkill(Grid grid)
    {
        if (enemyData.enemySkill != null)
        {
            enemyData.enemySkill.ActivateSkill(grid, gameObject);
        }
        else
        {
            Debug.Log($"{gameObject.name}���� �Ҵ�� ��ų�� �����ϴ�.");
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
        Debug.Log($"{gameObject.name}��(��) �׾����ϴ�!");

        
        if (spawner != null)
        {
            spawner.RemoveEnemy(gameObject);
        }
        else
        {
            Debug.LogError("EnemySpawner�� ã�� �� ���� ���� ����Ʈ���� �������� ���߽��ϴ�!");
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
