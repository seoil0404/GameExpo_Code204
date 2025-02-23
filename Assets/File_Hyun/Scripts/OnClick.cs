using UnityEngine;

public class OnClick : MonoBehaviour
{
    public void OnButtonClick()
    {
        // ���õ� ĳ������ �����͸� �����ɴϴ�.
        var characterData = CharacterManager.selectedCharacter.characterData;

        // �ñر� �������� �� á���� Ȯ���մϴ�.
        if (characterData.CurrentUltimateGauge >= characterData.MaxUltimateGauge)
        {
            if (characterData.ultimateSkill != null)
            {
                // �ñر� ��ų Ȱ��ȭ: Grid.instance�� ���ڷ� �����մϴ�.
                characterData.ultimateSkill.ActivateUltimate(CharacterManager.selectedCharacter, Grid.instance);
                // �ñر� ��� �� ������ �ʱ�ȭ �� ����
                characterData.CurrentUltimateGauge = 0;
                CharacterManager.SaveUltimateGauge();
                Debug.Log("�ñر� �ߵ�!");
            }
            else
            {
                Debug.LogWarning("�ñر� ��ų�� �Ҵ�Ǿ� ���� �ʽ��ϴ�.");
            }
        }
        else
        {
            Debug.Log("�ñر� �������� �� ���� �ʾҽ��ϴ�.");
        }
    }
}
