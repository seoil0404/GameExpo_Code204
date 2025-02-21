using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;

    public Text CharacterNameText;
    public Slider UltimateGaugeSlider;
    public Transform SpawnPoint;
    public Character[] CharacterDataList;
    private GameObject currentCharacterInstance;
    private Character selectedCharacter;

    void Start()
    {
        if (GameData.SelectedCharacterIndex <= 0 || GameData.SelectedCharacterIndex > CharacterDataList.Length)
        {
            GameData.SelectedCharacterIndex = 1;
        }

        selectedCharacter = CharacterDataList[GameData.SelectedCharacterIndex - 1];

        int savedHp = PlayerPrefs.GetInt("PlayerCurrentHp", selectedCharacter.characterData.MaxHp);
        selectedCharacter.characterData.CurrentHp = savedHp;

        if (currentCharacterInstance == null)
        {
            InitializeCharacter(selectedCharacter);
        }
        else
        {
            UpdateCharacterUI(selectedCharacter);
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

        character.characterData.Initialize();
        UpdateCharacterUI(character);

        Debug.Log($"[CharacterManager] 새로운 캐릭터 생성: {character.characterData.characterName}, HP: {character.characterData.CurrentHp}");
    }

    private void UpdateCharacterUI(Character character)
    {
        CharacterNameText.text = character.characterData.characterName;
        UltimateGaugeSlider.maxValue = character.characterData.ultimateGaugeMax;
        UltimateGaugeSlider.value = 0;

        Debug.Log($"[CharacterManager] 캐릭터 UI 업데이트: {character.characterData.characterName}");
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        selectedCharacter.characterData.CurrentHp -= totalDamage;


        PlayerPrefs.SetInt("PlayerCurrentHp", selectedCharacter.characterData.CurrentHp);
        PlayerPrefs.Save();

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
