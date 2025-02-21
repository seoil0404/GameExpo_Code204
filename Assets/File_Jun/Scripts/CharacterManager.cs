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
        InitializeCharacter(selectedCharacter);
    }

    public void InitializeCharacter(Character character)
    {
        if (currentCharacterInstance != null)
        {
            Destroy(currentCharacterInstance);
        }

        currentCharacterInstance = Instantiate(character.characterData.characterPrefab, SpawnPoint);

        RectTransform rectTransform = currentCharacterInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localPosition = Vector3.zero;
        }

        CharacterNameText.text = character.characterData.characterName;
        UltimateGaugeSlider.maxValue = character.characterData.ultimateGaugeMax;
        UltimateGaugeSlider.value = 0;
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp -= totalDamage;

        Debug.Log($"[CharacterManager] 플레이어가 {totalDamage} 데미지를 받았습니다. 현재 HP: {characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp}");

        if (characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp <= 0)
        {
            CharacterDied();
        }
    }

    private void CharacterDied()
    {
        Debug.Log("캐릭터가 죽었습니다!");
      
    }
}
