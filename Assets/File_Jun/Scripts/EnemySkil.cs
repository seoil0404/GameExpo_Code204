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
                Debug.Log($"{enemy.name}��(��) [�̳� ����] ��ų�� ����Ͽ� ����� �����ߴ�.");
                break;

            case SkillType.DestroyBlock:
                grid.DestroyRandomPlayerBlock();
                Debug.Log($"{enemy.name}��(��) [�̳� ����] ��ų�� ����Ͽ� �÷��̾��� ����� �ı��ߴ�.");
                break;

            case EnemySkill.SkillType.SealBlock:
                EnemyStats stats = enemy.GetComponent<EnemyStats>();
                if (stats != null && !stats.HasUsedEatBlock())
                {
                    GameObject block = grid.EatRandomBlock(enemy);
                    if (block != null)
                    {
                        stats.SetEatenBlock(block);
                        Debug.Log($"{enemy.name}��(��) [�̳� ����] ��ų�� ����Ͽ� ����� �Ծ� �����߽��ϴ�.");
                    }
                    else
                    {
                        Debug.Log($"{enemy.name}��(��) ���� ����� �����ϴ�.");
                    }
                }
                else
                {
                    Debug.Log($"{enemy.name}�� �̹� EatBlock ��ų(���Ρ������ �Ա�)�� ����߽��ϴ�.");
                }
                break;

        }
    }
}
