using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyData enemyData;
    private int hp;
    private int atk;
    private float dodgeChance;

    public void SetStats(int difficulty)
    {
        hp = enemyData.baseHP + (5 * difficulty);
        atk = enemyData.baseATK + (5 / Mathf.Max(difficulty % 4, 1));
        dodgeChance = enemyData.dodgeChance;
    }

    public bool TryDodge()
    {
        float roll = Random.Range(0f, 100f);
        return roll < dodgeChance;
    }
}
