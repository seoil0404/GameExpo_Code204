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
        hpText.text = $"{character.characterData.maxHp}/{character.characterData.maxHp}";
        ultimateGaugeSlider.maxValue = character.characterData.ultimateGaugeMax;
        ultimateGaugeSlider.value = 0;
    }

}
