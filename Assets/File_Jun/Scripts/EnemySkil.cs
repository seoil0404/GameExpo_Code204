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

            case SkillType.SealBlock:
                grid.SealRandomBlock(enemy);
                Debug.Log($"{enemy.name}��(��) [�̳� ����] ��ų�� ����Ͽ� ����� �����ߴ�.");
                break;
        }
    }
}
