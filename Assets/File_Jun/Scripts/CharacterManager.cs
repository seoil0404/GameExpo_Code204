using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;
    public Text CharacterNameText;
    public Slider UltimateGaugeSlider;
    public Transform SpawnPoint;
    private static GameObject currentCharacterInstance;
    public static Character selectedCharacter;
    private static int savedHp = -1;
    private const string HpKey = "SavedHp";
    private const string UltimateKey = "SavedUltimateGauge";

    void Start()
    {
        if (GameData.SelectedCharacterIndex <= 0 || GameData.SelectedCharacterIndex > characters.Length)
            GameData.SelectedCharacterIndex = 1;

        selectedCharacter = characters[GameData.SelectedCharacterIndex - 1];

        if (GameStartTracker.IsHavetobeReset)
        {
            Debug.Log("캐릭터 초기화 실행");
            ResetHp();
            SaveHp();
            ResetUltimateGauge();
            GameStartTracker.IsHavetobeReset = false;
        }

        LoadHp();
        LoadUltimateGauge();

        if (currentCharacterInstance == null)
            InitializeCharacter(selectedCharacter);
    }

    public void InitializeCharacter(Character character)
    {
        if (currentCharacterInstance != null)
        {
            Debug.Log($"[CharacterManager] 기존 캐릭터({currentCharacterInstance.name})가 이미 존재합니다.");
            return;
        }
        currentCharacterInstance = Instantiate(character.characterData.characterPrefab, SpawnPoint);
        RectTransform rectTransform = currentCharacterInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
            rectTransform.localPosition = Vector3.zero;
        Debug.Log($"[CharacterManager] 캐릭터 프리팹 생성: {character.characterData.CharacterName}");
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        if (selectedCharacter.characterData.NegateNextDamage)
        {
            // 피해 무효화 후, 피해의 절반만큼 회복
            selectedCharacter.characterData.ActivateLifeSteal(totalDamage);
            Debug.Log($"{selectedCharacter.characterData.CharacterName}의 궁극기 효과로 인해 {totalDamage} 데미지가 무효화되고, {totalDamage / 2} 만큼 회복되었습니다.");
            selectedCharacter.characterData.NegateNextDamage = false;
            return;
        }

        selectedCharacter.characterData.CurrentHp -= totalDamage;
        savedHp = selectedCharacter.characterData.CurrentHp;
        SaveHp();
        Debug.Log($"[CharacterManager] 플레이어가 {totalDamage} 데미지를 받았습니다. 현재 HP: {selectedCharacter.characterData.CurrentHp}");

        if (selectedCharacter.characterData.CurrentHp <= 0)
            CharacterDied();
    }

    private void CharacterDied()
    {
        Debug.Log("캐릭터가 죽었습니다!");
        GameStartTracker.IsHavetobeReset = false;
        Scene.Controller.LoadScene(Scene.MainScene);
    }

    public void SaveHp()
    {
        PlayerPrefs.SetInt(HpKey, selectedCharacter.characterData.CurrentHp);
        PlayerPrefs.Save();
        Debug.Log($"[CharacterManager] HP 저장됨: {selectedCharacter.characterData.CurrentHp}");
    }

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

    public void ResetHp()
    {
        selectedCharacter.characterData.CurrentHp = selectedCharacter.characterData.MaxHp;
        savedHp = selectedCharacter.characterData.MaxHp;
        SaveHp();
        Debug.Log($"[CharacterManager] HP 초기화됨: {selectedCharacter.characterData.CurrentHp}");
    }

    public static void SaveUltimateGauge()
    {
        PlayerPrefs.SetInt(UltimateKey, selectedCharacter.characterData.CurrentUltimateGauge);
        PlayerPrefs.Save();
        Debug.Log($"[CharacterManager] 궁극기 게이지 저장됨: {selectedCharacter.characterData.CurrentUltimateGauge}");
    }

    public void LoadUltimateGauge()
    {
        if (PlayerPrefs.HasKey(UltimateKey))
        {
            selectedCharacter.characterData.CurrentUltimateGauge = PlayerPrefs.GetInt(UltimateKey);
            Debug.Log($"[CharacterManager] 저장된 궁극기 게이지 불러옴: {selectedCharacter.characterData.CurrentUltimateGauge}");
        }
        else
        {
            selectedCharacter.characterData.CurrentUltimateGauge = 0;
        }
    }

    public void ResetUltimateGauge()
    {
        selectedCharacter.characterData.CurrentUltimateGauge = 0;
        SaveUltimateGauge();
        Debug.Log($"[CharacterManager] 궁극기 게이지 초기화됨: {selectedCharacter.characterData.CurrentUltimateGauge}");
    }
}
