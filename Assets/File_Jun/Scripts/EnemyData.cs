using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int baseHP;
    public int baseATK;
    public Sprite enemySprite;
    [Range(0, 100)] public float dodgeChance;
    public EnemySkill enemySkill;
}
