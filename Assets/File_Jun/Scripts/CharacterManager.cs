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
    private const string HpKey = "SavedHp"; // PlayerPrefs Ű

    void Start()
    {
        if (GameData.SelectedCharacterIndex <= 0 || GameData.SelectedCharacterIndex > characters.Length)
        {
            GameData.SelectedCharacterIndex = 1;
        }

        selectedCharacter = characters[GameData.SelectedCharacterIndex - 1];

        // ����� HP �ҷ�����
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
            Debug.Log($"[CharacterManager] ���� ĳ����({currentCharacterInstance.name})�� �̹� �����մϴ�. ���ο� ĳ���͸� �������� �ʽ��ϴ�.");
            return;
        }

        currentCharacterInstance = Instantiate(character.characterData.characterPrefab, SpawnPoint);

        RectTransform rectTransform = currentCharacterInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localPosition = Vector3.zero;
        }

        Debug.Log($"[CharacterManager] ĳ���� ������ ����: {character.characterData.CharacterName}");
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        selectedCharacter.characterData.CurrentHp -= totalDamage;
        savedHp = selectedCharacter.characterData.CurrentHp;
        SaveHp();

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

    /// <summary>
    /// ���� HP�� �����ϴ� �Լ�
    /// </summary>
    public void SaveHp()
    {
        PlayerPrefs.SetInt(HpKey, selectedCharacter.characterData.CurrentHp);
        PlayerPrefs.Save();
        Debug.Log($"[CharacterManager] HP �����: {selectedCharacter.characterData.CurrentHp}");
    }

    /// <summary>
    /// ����� HP�� �ҷ����� �Լ�
    /// </summary>
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

    /// <summary>
    /// HP�� �ʱ�ȭ�ϴ� �Լ� (ĳ������ �⺻ HP�� ����)
    /// </summary>
    public void ResetHp()
    {
        selectedCharacter.characterData.CurrentHp = selectedCharacter.characterData.MaxHp;
        savedHp = selectedCharacter.characterData.MaxHp;
        SaveHp();
        Debug.Log($"[CharacterManager] HP �ʱ�ȭ��: {selectedCharacter.characterData.CurrentHp}");
    }
}
