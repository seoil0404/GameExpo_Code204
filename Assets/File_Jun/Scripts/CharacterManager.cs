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
            Debug.Log($"[CharacterManager] 기존 캐릭터({currentCharacterInstance.name})가 이미 존재합니다. 새로운 캐릭터를 생성하지 않습니다.");
            return;
        }

  
        currentCharacterInstance = Instantiate(character.characterData.characterPrefab, SpawnPoint);

        RectTransform rectTransform = currentCharacterInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localPosition = Vector3.zero;
        }

        Debug.Log($"[CharacterManager] 캐릭터 프리팹 생성: {character.characterData.characterName}");
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        selectedCharacter.characterData.CurrentHp -= totalDamage;
        savedHp = selectedCharacter.characterData.CurrentHp;

        Debug.Log($"[CharacterManager] 플레이어가 {totalDamage} 데미지를 받았습니다. 현재 HP: {selectedCharacter.characterData.CurrentHp}");

        if (selectedCharacter.characterData.CurrentHp <= 0)
        {
            CharacterDied();
        }
    }

    private void CharacterDied()
    {
        Debug.Log("캐릭터가 죽었습니다!");
    }
}
