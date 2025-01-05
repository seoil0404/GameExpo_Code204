using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text hpText;
    public Slider ultimateGaugeSlider;
    public Transform spawnPoint;
    public Character[] characterDataList;

    private GameObject currentCharacterInstance;
    private Character selectedCharacter;
    private int currentHp;

    void Start()
    {
        if (GameData.SelectedCharacterIndex <= 0 || GameData.SelectedCharacterIndex > characterDataList.Length)
        {
            GameData.SelectedCharacterIndex = 1;
        }

        selectedCharacter = characterDataList[GameData.SelectedCharacterIndex - 1];
        InitializeCharacter(selectedCharacter);
    }

    public void InitializeCharacter(Character character)
    {
        if (currentCharacterInstance != null)
        {
            Destroy(currentCharacterInstance);
        }

        currentCharacterInstance = Instantiate(character.characterData.characterPrefab, spawnPoint);

        RectTransform rectTransform = currentCharacterInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localPosition = Vector3.zero;
        }

        playerNameText.text = character.characterData.characterName;
        currentHp = character.characterData.maxHp;
        UpdateHpText();
        ultimateGaugeSlider.maxValue = character.characterData.ultimateGaugeMax;
        ultimateGaugeSlider.value = 0;
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
        hpText.text = $"HP: {currentHp}";
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
