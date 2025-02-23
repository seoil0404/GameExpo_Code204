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
            Debug.Log("ĳ���� �ʱ�ȭ ����");
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
            Debug.Log($"[CharacterManager] ���� ĳ����({currentCharacterInstance.name})�� �̹� �����մϴ�.");
            return;
        }
        currentCharacterInstance = Instantiate(character.characterData.characterPrefab, SpawnPoint);
        RectTransform rectTransform = currentCharacterInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
            rectTransform.localPosition = Vector3.zero;
        Debug.Log($"[CharacterManager] ĳ���� ������ ����: {character.characterData.CharacterName}");
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        if (selectedCharacter.characterData.NegateNextDamage)
        {
            // ���� ��ȿȭ ��, ������ ���ݸ�ŭ ȸ��
            selectedCharacter.characterData.ActivateLifeSteal(totalDamage);
            Debug.Log($"{selectedCharacter.characterData.CharacterName}�� �ñر� ȿ���� ���� {totalDamage} �������� ��ȿȭ�ǰ�, {totalDamage / 2} ��ŭ ȸ���Ǿ����ϴ�.");
            selectedCharacter.characterData.NegateNextDamage = false;
            return;
        }

        selectedCharacter.characterData.CurrentHp -= totalDamage;
        savedHp = selectedCharacter.characterData.CurrentHp;
        SaveHp();
        Debug.Log($"[CharacterManager] �÷��̾ {totalDamage} �������� �޾ҽ��ϴ�. ���� HP: {selectedCharacter.characterData.CurrentHp}");

        if (selectedCharacter.characterData.CurrentHp <= 0)
            CharacterDied();
    }

    private void CharacterDied()
    {
        Debug.Log("ĳ���Ͱ� �׾����ϴ�!");
        GameStartTracker.IsHavetobeReset = false;
        Scene.Controller.LoadScene(Scene.MainScene);
    }

    public void SaveHp()
    {
        PlayerPrefs.SetInt(HpKey, selectedCharacter.characterData.CurrentHp);
        PlayerPrefs.Save();
        Debug.Log($"[CharacterManager] HP �����: {selectedCharacter.characterData.CurrentHp}");
    }

    public void LoadHp()
    {
        if (PlayerPrefs.HasKey(HpKey))
        {
            savedHp = PlayerPrefs.GetInt(HpKey);
            selectedCharacter.characterData.CurrentHp = savedHp;
            Debug.Log($"[CharacterManager] ����� HP �ҷ���: {savedHp}");
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
        Debug.Log($"[CharacterManager] HP �ʱ�ȭ��: {selectedCharacter.characterData.CurrentHp}");
    }

    public static void SaveUltimateGauge()
    {
        PlayerPrefs.SetInt(UltimateKey, selectedCharacter.characterData.CurrentUltimateGauge);
        PlayerPrefs.Save();
        Debug.Log($"[CharacterManager] �ñر� ������ �����: {selectedCharacter.characterData.CurrentUltimateGauge}");
    }

    public void LoadUltimateGauge()
    {
        if (PlayerPrefs.HasKey(UltimateKey))
        {
            selectedCharacter.characterData.CurrentUltimateGauge = PlayerPrefs.GetInt(UltimateKey);
            Debug.Log($"[CharacterManager] ����� �ñر� ������ �ҷ���: {selectedCharacter.characterData.CurrentUltimateGauge}");
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
        Debug.Log($"[CharacterManager] �ñر� ������ �ʱ�ȭ��: {selectedCharacter.characterData.CurrentUltimateGauge}");
    }
}
