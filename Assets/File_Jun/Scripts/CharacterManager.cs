using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;

    public Text CharacterNameText;
    public Slider UltimateGaugeSlider;
    public Transform SpawnPoint;
    private static GameObject currentCharacterInstance;
    private static Character selectedCharacter;

    private static int savedHp = -1;

    void Awake()
    {
        if (GameData.SelectedCharacterIndex <= 0 || GameData.SelectedCharacterIndex > characters.Length)
        {
            GameData.SelectedCharacterIndex = 1;
        }

        selectedCharacter = characters[GameData.SelectedCharacterIndex - 1];

        
        if (savedHp == -1)
        {
            savedHp = selectedCharacter.characterData.CurrentHp;
        }
        else
        {
            selectedCharacter.characterData.CurrentHp = savedHp; 
        }

        if (currentCharacterInstance == null)
        {
            InitializeCharacter(selectedCharacter);
        }
    }

    public void InitializeCharacter(Character character)
    {
        if (currentCharacterInstance != null)
        {
            Debug.Log($"[CharacterManager] ���� ĳ����({currentCharacterInstance.name})�� �̹� �����մϴ�. ���ο� ĳ���͸� �������� �ʽ��ϴ�.");
            return;
        }

  
        currentCharacterInstance = Instantiate(character.characterData.characterPrefab, SpawnPoint);

        RectTransform rectTransform = currentCharacterInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localPosition = Vector3.zero;
        }

        Debug.Log($"[CharacterManager] ĳ���� ������ ����: {character.characterData.characterName}");
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        selectedCharacter.characterData.CurrentHp -= totalDamage;
        savedHp = selectedCharacter.characterData.CurrentHp;

        Debug.Log($"[CharacterManager] �÷��̾ {totalDamage} �������� �޾ҽ��ϴ�. ���� HP: {selectedCharacter.characterData.CurrentHp}");

        if (selectedCharacter.characterData.CurrentHp <= 0)
        {
            CharacterDied();
        }
    }

    private void CharacterDied()
    {
        Debug.Log("ĳ���Ͱ� �׾����ϴ�!");
    }
}
