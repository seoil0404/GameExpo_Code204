using UnityEngine;

[CreateAssetMenu(fileName = "CharacterUltimate", menuName = "Character/UltimateSkill")]
public class UltimateSkill : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public enum UltimateType
    {
        StackAndDouble,           
        NegateDamageCureLifeSteal,
        EvolveSwordIncreaseExecution,
        DropBlocksAndDoubleDamage 
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
                character.characterData.IsInvincible = true;
                EffectManager.Instance.SpawnShield(CharacterManager.currentCharacterInstance);
                character.characterData.CureStatusEffects();
                character.characterData.NextAttackLifeSteal = true;
                Debug.Log($"{character.characterData.CharacterName}�� �ñر� [{skillName}] �ߵ�: ��ȿȭ + �����̻� ġ�� + ���� Ȱ��ȭ");
                break;


            case UltimateType.EvolveSwordIncreaseExecution:
                character.characterData.ExecutionRate += 5;
                grid.additionalExecutionDamage = character.characterData.ExecutionRate;
                Debug.Log($"{character.characterData.CharacterName}�� �ñر� [{skillName}] (EvolveSwordIncreaseExecution) �ߵ���. �� ó����: {character.characterData.ExecutionRate}");
                break;

            case UltimateType.DropBlocksAndDoubleDamage:
                grid.DropAllBlocks();
                grid.ultimateDamageMultiplier = 2f;
                Debug.Log($"{character.characterData.CharacterName}�� �ñر� [{skillName}] (DropBlocksAndDoubleDamage) �ߵ���.");
                break;
        }
    }
}
