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

            case SkillType.SealBlock:
                grid.SealRandomBlock(enemy);
                Debug.Log($"{enemy.name}이(가) [미노 방해] 스킬을 사용하여 블록을 봉인했다.");
                break;
        }
    }
}
