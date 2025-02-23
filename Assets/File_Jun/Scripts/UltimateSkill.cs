using UnityEngine;

[CreateAssetMenu(fileName = "CharacterUltimate", menuName = "Character/UltimateSkill")]
public class UltimateSkill : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public enum UltimateType
    {
        StackAndDouble,            // 기존: 블록을 아래로 쌓고 클리어 데미지 2배
        NegateDamageCureLifeSteal, // 다음 피해 무효화 + 상태 이상 치료 + 흡혈(피해의 절반 회복)
        EvolveSwordIncreaseExecution, // 검 진화: 처형율 +5, 다음 공격에 처형율 추가 데미지 적용
        DropBlocksAndDoubleDamage  // 새로 추가: 판에 배치된 블록들을 아래로 떨어뜨리고 클리어 데미지 2배
    }
    public UltimateType ultimateType;

    public void ActivateUltimate(Character character, Grid grid)
    {
        switch (ultimateType)
        {
            case UltimateType.StackAndDouble:
                grid.DropAllBlocks();
                grid.ultimateDamageMultiplier = 2f;
                Debug.Log($"{character.characterData.CharacterName}의 궁극기 [{skillName}] (StackAndDouble) 발동됨.");
                break;

            case UltimateType.NegateDamageCureLifeSteal:
                character.characterData.NegateNextDamage = true;
                character.characterData.CureStatusEffects();
                Debug.Log($"{character.characterData.CharacterName}의 궁극기 [{skillName}] (NegateDamageCureLifeSteal) 발동됨.");
                break;

            case UltimateType.EvolveSwordIncreaseExecution:
                character.characterData.ExecutionRate += 5;
                grid.additionalExecutionDamage = character.characterData.ExecutionRate;
                Debug.Log($"{character.characterData.CharacterName}의 궁극기 [{skillName}] (EvolveSwordIncreaseExecution) 발동됨. 새 처형율: {character.characterData.ExecutionRate}");
                break;

            case UltimateType.DropBlocksAndDoubleDamage:
                grid.DropAllBlocks(); // 판 위의 블록들을 아래로 떨어뜨림
                grid.ultimateDamageMultiplier = 2f; // 클리어 데미지 2배 적용
                Debug.Log($"{character.characterData.CharacterName}의 궁극기 [{skillName}] (DropBlocksAndDoubleDamage) 발동됨.");
                break;
        }
    }
}
