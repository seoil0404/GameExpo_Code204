using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemyStats : MonoBehaviour
{
    public EnemyData enemyData; 
    public TextMeshProUGUI healthText; 

    private int hp;
    private int atk;
    private float dodgeChance;

    public void SetStats(int difficulty)
    {
        hp = enemyData.baseHP + (5 * difficulty);
        atk = enemyData.baseATK + (5 / Mathf.Max(difficulty % 4, 1));
        dodgeChance = enemyData.dodgeChance;
        UpdateHealthText();
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
        Destroy(gameObject);
    }
    public int GetAttackDamage()
    {
        return atk;
    }

}
