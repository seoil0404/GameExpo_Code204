using UnityEngine;

[CreateAssetMenu(fileName = "CharacterUltimate", menuName = "Character/UltimateSkill")]
public class UltimateSkill : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public enum UltimateType
    {
        StackAndDouble,            // ����: ����� �Ʒ��� �װ� Ŭ���� ������ 2��
        NegateDamageCureLifeSteal, // ���� ���� ��ȿȭ + ���� �̻� ġ�� + ����(������ ���� ȸ��)
        EvolveSwordIncreaseExecution, // �� ��ȭ: ó���� +5, ���� ���ݿ� ó���� �߰� ������ ����
        DropBlocksAndDoubleDamage  // ���� �߰�: �ǿ� ��ġ�� ��ϵ��� �Ʒ��� ����߸��� Ŭ���� ������ 2��
    }
    public UltimateType ultimateType;

    public void ActivateUltimate(Character character, Grid grid)
    {
        switch (ultimateType)
        {
            case UltimateType.StackAndDouble:
                grid.DropAllBlocks();
                grid.ultimateDamageMultiplier = 2f;
                Debug.Log($"{character.characterData.CharacterName}�� �ñر� [{skillName}] (StackAndDouble) �ߵ���.");
                break;

            case UltimateType.NegateDamageCureLifeSteal:
                character.characterData.NegateNextDamage = true;
                character.characterData.CureStatusEffects();
                Debug.Log($"{character.characterData.CharacterName}�� �ñر� [{skillName}] (NegateDamageCureLifeSteal) �ߵ���.");
                break;

            case UltimateType.EvolveSwordIncreaseExecution:
                character.characterData.ExecutionRate += 5;
                grid.additionalExecutionDamage = character.characterData.ExecutionRate;
                Debug.Log($"{character.characterData.CharacterName}�� �ñر� [{skillName}] (EvolveSwordIncreaseExecution) �ߵ���. �� ó����: {character.characterData.ExecutionRate}");
                break;

            case UltimateType.DropBlocksAndDoubleDamage:
                grid.DropAllBlocks(); // �� ���� ��ϵ��� �Ʒ��� ����߸�
                grid.ultimateDamageMultiplier = 2f; // Ŭ���� ������ 2�� ����
                Debug.Log($"{character.characterData.CharacterName}�� �ñر� [{skillName}] (DropBlocksAndDoubleDamage) �ߵ���.");
                break;
        }
    }
}
