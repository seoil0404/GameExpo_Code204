using UnityEngine;

[CreateAssetMenu(fileName = "EnemySkill", menuName = "Enemy/EnemySkill")]
public class EnemySkill : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public enum SkillType { SpawnBlock, DestroyBlock, SealBlock, DestroyArea, PowerUp, SwallowBlock, Poison, Minotroll }
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

            case SkillType.SwallowBlock:
                if (enemyStats != null && !enemyStats.HasUsedSwallowBlock)
                {
                    ShapeStorage shapeStorage = FindFirstObjectByType<ShapeStorage>();
                    if (shapeStorage != null)
                    {
                        Shape targetShape = shapeStorage.GetRandomActiveShape();
                        if (targetShape != null)
                        {
                            targetShape.DeactivateEntireShape();
                            enemyStats.SetSwallowBlockUsed();
                            Debug.Log($"{enemy.name}이(가) [블록 삼키기] 스킬을 사용하고 EnemySkill을 비웠습니다.");
                        }
                        else
                        {
                            Debug.LogWarning("[SwallowBlock] 활성화된 블록 모양이 없어 스킬을 사용할 수 없습니다.");
                        }
                    }
                    else
                    {
                        Debug.LogError("[SwallowBlock] ShapeStorage를 찾을 수 없습니다.");
                    }
                }
                else
                {
                    Debug.LogWarning("[SwallowBlock] 이미 사용된 스킬입니다. 다시 사용할 수 없습니다.");
                }
                break;


            case SkillType.Poison:
                if (enemyStats != null)
                {
                    int poisonDamage = enemyStats.GetAttack();
                    enemyStats.ApplyPoison(poisonDamage);
                    Debug.Log($"{enemy.name}이(가) [독 공격]을 사용! 피해: {poisonDamage / 2}, 지속시간: {poisonDamage / 2}");
                }
                break;

            case SkillType.Minotroll:
                if (enemyStats != null)
                {
                    enemyStats.IncreaseATKByOne();
                    Debug.Log($"{enemy.name}이(가) [미노 트롤] 스킬을 사용하여 ATK +1 증가! 현재 ATK: {enemyStats.GetAttack()}");
                }
                break;

        }
    }
}
