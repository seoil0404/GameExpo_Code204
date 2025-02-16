using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public enum HabitatType { Forest, Castle, DevilCastle }
    public enum EnemyType { Common, Boss }

    public string enemyName;
    public int baseHP;
    public int baseATK;
    public Sprite enemySprite;
    [Range(0, 100)] public float dodgeChance;
    public EnemySkill enemySkill;

    public HabitatType habitat;
    public EnemyType enemyType; 
}
