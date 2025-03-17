using UnityEngine;

[CreateAssetMenu(fileName = "EnemySkill", menuName = "Enemy/EnemySkill")]
public class EnemySkill : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public enum SkillType { SpawnBlock, DestroyBlock, SealBlock, DestroyArea, PowerUp, SwallowBlock }
    public SkillType skillType;

    public void ActivateSkill(Grid grid, GameObject enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        switch (skillType)
        {
            case SkillType.SpawnBlock:
                grid.SpawnRandomBlock();
                Debug.Log($"{enemy.name}��(��) [�̳� ����] ��ų�� ����Ͽ� ����� �����ߴ�.");
                break;

            case SkillType.DestroyBlock:
                grid.DestroyRandomPlayerBlock();
                Debug.Log($"{enemy.name}��(��) [�̳� ����] ��ų�� ����Ͽ� �÷��̾��� ����� �ı��ߴ�.");
                break;

            case SkillType.DestroyArea:
                grid.DeactivateRandom4x4();
                Debug.Log($"{enemy.name}��(��) [�̳� ����] ��ų�� ����Ͽ� 4x4 ����� �����ߴ�.");
                break;

            case SkillType.PowerUp:
                if (enemyStats != null)
                {
                    enemyStats.IncreaseATK();
                }
                Debug.Log($"{enemy.name}��(��) [�� ����] ��ų�� ����Ͽ� ATK�� ��ȭ�ߴ�.");
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
                            Debug.Log($"{enemy.name}��(��) [��� ��Ű��] ��ų�� ����ϰ� EnemySkill�� ������ϴ�.");
                        }
                        else
                        {
                            Debug.LogWarning("[SwallowBlock] Ȱ��ȭ�� ��� ����� ���� ��ų�� ����� �� �����ϴ�.");
                        }
                    }
                    else
                    {
                        Debug.LogError("[SwallowBlock] ShapeStorage�� ã�� �� �����ϴ�.");
                    }
                }
                else
                {
                    Debug.LogWarning("[SwallowBlock] �̹� ���� ��ų�Դϴ�. �ٽ� ����� �� �����ϴ�.");
                }
                break;




        }
    }
}
