using UnityEngine;

[CreateAssetMenu(fileName = "EnemySkill", menuName = "Enemy/EnemySkill")]
public class EnemySkill : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public enum SkillType { SpawnBlock, DestroyBlock, SealBlock, DestroyArea, PowerUp }
    public SkillType skillType;

    public void ActivateSkill(Grid grid, GameObject enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        switch (skillType)
        {
            case SkillType.SpawnBlock:
                grid.SpawnRandomBlock();
                Debug.Log($"{enemy.name}이(가) [미노 방해] 스킬을 사용하여 블록을 생성했다.");
                break;

            case SkillType.DestroyBlock:
                grid.DestroyRandomPlayerBlock();
                Debug.Log($"{enemy.name}이(가) [미노 방해] 스킬을 사용하여 플레이어의 블록을 파괴했다.");
                break;

            case SkillType.DestroyArea:
                grid.DeactivateRandom4x4();
                Debug.Log($"{enemy.name}이(가) [미노 방해] 스킬을 사용하여 4x4 블록을 제거했다.");
                break;

            case SkillType.PowerUp:
                if (enemyStats != null)
                {
                    enemyStats.IncreaseATK();
                }
                Debug.Log($"{enemy.name}이(가) [힘 증가] 스킬을 사용하여 ATK를 강화했다.");
                break;
        }
    }
}
