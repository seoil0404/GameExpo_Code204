using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public Text CharacterNameText;
    public Text PlayerHealthText;
    public Slider UltimateGaugeSlider;
    public Transform SpawnPoint;
    public Character[] CharacterDataList;

    private GameObject currentCharacterInstance;
    private Character selectedCharacter;
    private int currentHp;

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
        currentHp = character.characterData.maxHp;
        UpdateHpText();
        UltimateGaugeSlider.maxValue = character.characterData.ultimateGaugeMax;
        UltimateGaugeSlider.value = 0;
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        currentHp -= totalDamage;

        if (currentHp <= 0)
        {
            currentHp = 0;
            CharacterDied();
        }

        UpdateHpText();
    }

    private void UpdateHpText()
    {
        PlayerHealthText.text = $"HP: {currentHp}";
    }

    public int GetCurrentHp()
    {
        return currentHp;
    }

    private void CharacterDied()
    {
        Debug.Log("캐릭터가 죽었습니다!");
        
    }
}
