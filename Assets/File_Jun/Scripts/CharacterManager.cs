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
    private const string HpKey = "SavedHp"; // PlayerPrefs 키

    void Start()
    {
        if (GameData.SelectedCharacterIndex <= 0 || GameData.SelectedCharacterIndex > characters.Length)
        {
            GameData.SelectedCharacterIndex = 1;
        }

        selectedCharacter = characters[GameData.SelectedCharacterIndex - 1];

        // 저장된 HP 불러오기
        LoadHp();

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

        Debug.Log($"[CharacterManager] 캐릭터 프리팹 생성: {character.characterData.CharacterName}");
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        selectedCharacter.characterData.CurrentHp -= totalDamage;
        savedHp = selectedCharacter.characterData.CurrentHp;
        SaveHp();

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

    /// <summary>
    /// 현재 HP를 저장하는 함수
    /// </summary>
    public void SaveHp()
    {
        PlayerPrefs.SetInt(HpKey, selectedCharacter.characterData.CurrentHp);
        PlayerPrefs.Save();
        Debug.Log($"[CharacterManager] HP 저장됨: {selectedCharacter.characterData.CurrentHp}");
    }

    /// <summary>
    /// 저장된 HP를 불러오는 함수
    /// </summary>
    public void LoadHp()
    {
        if (PlayerPrefs.HasKey(HpKey))
        {
            savedHp = PlayerPrefs.GetInt(HpKey);
            selectedCharacter.characterData.CurrentHp = savedHp;
            Debug.Log($"[CharacterManager] 저장된 HP 불러옴: {savedHp}");
        }
        else
        {
            savedHp = selectedCharacter.characterData.CurrentHp;
        }
    }

    /// <summary>
    /// HP를 초기화하는 함수 (캐릭터의 기본 HP로 복원)
    /// </summary>
    public void ResetHp()
    {
        selectedCharacter.characterData.CurrentHp = selectedCharacter.characterData.MaxHp;
        savedHp = selectedCharacter.characterData.MaxHp;
        SaveHp();
        Debug.Log($"[CharacterManager] HP 초기화됨: {selectedCharacter.characterData.CurrentHp}");
    }
}
