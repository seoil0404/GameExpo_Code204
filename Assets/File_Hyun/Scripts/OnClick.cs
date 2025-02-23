using UnityEngine;

public class OnClick : MonoBehaviour
{
    public void OnButtonClick()
    {
        // 선택된 캐릭터의 데이터를 가져옵니다.
        var characterData = CharacterManager.selectedCharacter.characterData;

        // 궁극기 게이지가 꽉 찼는지 확인합니다.
        if (characterData.CurrentUltimateGauge >= characterData.MaxUltimateGauge)
        {
            if (characterData.ultimateSkill != null)
            {
                // 궁극기 스킬 활성화: Grid.instance을 인자로 전달합니다.
                characterData.ultimateSkill.ActivateUltimate(CharacterManager.selectedCharacter, Grid.instance);
                // 궁극기 사용 후 게이지 초기화 및 저장
                characterData.CurrentUltimateGauge = 0;
                CharacterManager.SaveUltimateGauge();
                Debug.Log("궁극기 발동!");
            }
            else
            {
                Debug.LogWarning("궁극기 스킬이 할당되어 있지 않습니다.");
            }
        }
        else
        {
            Debug.Log("궁극기 게이지가 다 차지 않았습니다.");
        }
    }
}
