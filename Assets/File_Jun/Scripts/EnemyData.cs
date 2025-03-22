using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public enum HabitatType { Forest, Castle, DevilCastle }
    public enum EnemyType { Common, SpecialCombat, Boss }

    public enum DefaultAttackType
    {
        Normal,
        ThornAttack
    }

    public string enemyName;
    public int baseHP;
    public int baseATK;
    public Sprite enemySprite;
    [Range(0, 100)] public float dodgeChance;
    public List<EnemySkill> enemySkills;

    public HabitatType habitat;
    public EnemyType enemyType;

    public DefaultAttackType defaultAttackType = DefaultAttackType.Normal;


}   
 