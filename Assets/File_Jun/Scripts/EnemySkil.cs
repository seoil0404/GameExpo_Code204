using UnityEngine;

[CreateAssetMenu(fileName = "EnemySkill", menuName = "Enemy/EnemySkill")]
public class EnemySkill : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public enum SkillType { SpawnBlock, DestroyBlock, SealBlock }
    public SkillType skillType;

    public void ActivateSkill(Grid grid, GameObject enemy)
    {
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

            case EnemySkill.SkillType.SealBlock:
                EnemyStats stats = enemy.GetComponent<EnemyStats>();
                if (stats != null && !stats.HasUsedEatBlock())
                {
                    GameObject block = grid.EatRandomBlock(enemy);
                    if (block != null)
                    {
                        stats.SetEatenBlock(block);
                        Debug.Log($"{enemy.name}이(가) [미노 방해] 스킬을 사용하여 블록을 먹어 제거했습니다.");
                    }
                    else
                    {
                        Debug.Log($"{enemy.name}이(가) 먹을 블록이 없습니다.");
                    }
                }
                else
                {
                    Debug.Log($"{enemy.name}은 이미 EatBlock 스킬(봉인→실제론 먹기)을 사용했습니다.");
                }
                break;

        }
    }
}
