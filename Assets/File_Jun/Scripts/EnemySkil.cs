using UnityEngine;

[CreateAssetMenu(fileName = "EnemySkill", menuName = "Enemy/EnemySkill")]
public class EnemySkill : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public enum SkillType { SpawnBlock, DestroyBlock, SealBlock, DestroyArea, PowerUp, SwallowBlock, Poison, Minotroll, Minokwizard1, Minokwizard2, ThornAttack, PetrifyBlockArea, Heal20Percent, SpawnSpecialMino, SpawnHealingMino }
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


            case SkillType.Poison:
                if (enemyStats != null)
                {
                    int poisonDamage = enemyStats.GetAttack();
                    enemyStats.ApplyPoison(poisonDamage);
                    Debug.Log($"{enemy.name}��(��) [�� ����]�� ���! ����: {poisonDamage / 2}, ���ӽð�: {poisonDamage / 2}");
                }
                break;

            case SkillType.Minotroll:
                if (enemyStats != null)
                {
                    enemyStats.IncreaseATKByOne();
                    Debug.Log($"{enemy.name}��(��) [�̳� Ʈ��] ��ų�� ����Ͽ� ATK +1 ����! ���� ATK: {enemyStats.GetAttack()}");
                }
                break;

            case SkillType.Minokwizard1:
                if (enemyStats != null)
                {
                    enemyStats.ActivateDamageMultiplier();
                    Debug.Log($"{enemy.name}��(��) [�̳�ũ ������] ��ų�� ����Ͽ� �� �� ���� 1.2�� ���� ����!");
                }
                break;

            case SkillType.Minokwizard2:
                HoldShape holdShape = FindFirstObjectByType<HoldShape>();
                if (holdShape != null)
                {
                    holdShape.ToggleShapeLock();
                    Debug.Log($"{enemy.name}��(��) [Minokwizard2] ��ų�� ����Ͽ� ����� �����Ϸ��� �õ��߽��ϴ�.");
                }
                else
                {
                    Debug.LogWarning("[Minokwizard2] HoldShape�� ã�� �� �����ϴ�!");
                }
                break;

            case SkillType.ThornAttack:
                if (enemyStats != null && grid != null)
                {
                    int thornDamage = Mathf.RoundToInt(enemyStats.GetAttack() / 2f);
                    CharacterManager.instance.ApplyDamageToCharacter(thornDamage);
                    enemyStats.IncreaseThorn();

                    Debug.Log($"[{enemy.name}]��(��) [���� ����] ��ų ���! �÷��̾�� {thornDamage} ������ + ���� 1 ����");
                }
                break;

            case SkillType.PetrifyBlockArea:
                grid.Spawn3x3PetrifiedBlocks();
                Debug.Log($"{enemy.name}��(��) [�̳� ��ȭ] ��ų�� ����Ͽ� 3x3 ��ȭ ����� �����ߴ�.");
                break;

            case SkillType.Heal20Percent:
                if (enemyStats != null)
                {
                    enemyStats.HealByPercentage(0.2f);
                    Debug.Log($"{enemy.name}��(��) [ȸ��] ��ų�� ����Ͽ� �ִ� ü���� 20%�� ȸ���߽��ϴ�.");
                }
                break;

            case SkillType.SpawnSpecialMino:
                if (enemyStats != null && grid != null)
                {
                    grid.SpawnSpecialMino(enemy);
                    Debug.Log($"{enemy.name}��(��) [Ư�� �̳�]�� ���忡 �����߽��ϴ�.");
                }
                break;

            case SkillType.SpawnHealingMino:
                if (enemyStats != null && grid != null)
                {
                    grid.SpawnHealingMino(enemy);
                    Debug.Log($"{enemy.name}��(��) [�� �̳�]�� ���忡 �����߽��ϴ�.");
                }
                break;





        }
    }
}
