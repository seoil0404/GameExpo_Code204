using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private GoldData goldData;

    public static CharacterManager instance;

    [SerializeField] private Character[] characters = null;
    public Text CharacterNameText;
    public Slider UltimateGaugeSlider;
    public Transform SpawnPoint;
    public static GameObject currentCharacterInstance;
    public static Character selectedCharacter;
    [SerializeField] private GameObject scoreScreen;
    private static int savedHp = -1;
    private const string HpKey = "SavedHp";
    private const string UltimateKey = "SavedUltimateGauge";
    private int lastCheckedHpBonus = 0;
    public float CharacterBaseDodgeChance = 0f;




    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (GameData.SelectedCharacterIndex <= 0 || GameData.SelectedCharacterIndex > characters.Length)
            GameData.SelectedCharacterIndex = 1;

        selectedCharacter = characters[GameData.SelectedCharacterIndex - 1];
        var treasureEffect = Object.FindFirstObjectByType<TreasureEffect>();
        if (GameStartTracker.IsHavetobeReset)
        {
            Debug.Log("ĳ���� �ʱ�ȭ ����");
            selectedCharacter.characterData.Initialize();
            ResetHp();
            SaveHp();
            ResetUltimateGauge();
            GameStartTracker.IsHavetobeReset = false;
            if (treasureEffect.Condemnation)
            {
                selectedCharacter.characterData.ExecutionRate += 5;
                Debug.Log("Condemnation ���� ȿ���� ó���� +5 ������");
            }
        }

        if (treasureEffect.NobleBlood)
        {
            int recoverAmount = Mathf.Max(1, selectedCharacter.characterData.CurrentHp / 10);
            selectedCharacter.characterData.CurrentHp += recoverAmount;

            if (selectedCharacter.characterData.CurrentHp > selectedCharacter.characterData.MaxHp)
                selectedCharacter.characterData.CurrentHp = selectedCharacter.characterData.MaxHp;

            SaveHp();
            Debug.Log($"[NobleBlood] ���� ȿ���� ���� ü���� 1/10({recoverAmount}) ȸ����. ���� HP: {selectedCharacter.characterData.CurrentHp}");
        }

        if (treasureEffect.HolyShield)
        {
            selectedCharacter.characterData.IsInvincible = true;
            Debug.Log("[HolyShield] ���� ȿ���� ��ȿȭ ���°� ���� �� 1�ϰ� ����˴ϴ�.");
        }

        LoadHp();
        LoadUltimateGauge();

        if (currentCharacterInstance == null)
            InitializeCharacter(selectedCharacter);

        if (treasureEffect.MoneyBack && !GameStartTracker.IsUsedMoneyBag)
        {
            GameStartTracker.IsUsedMoneyBag = true;
            goldData.InGameGold += 100;
            Debug.Log("MoneyBag ���� ȿ���� ��� 100 ����!");
        }

        if (treasureEffect.ShoesOfHermes)
        {
            CharacterBaseDodgeChance = 10f;
            Debug.Log("ShoesOfHermes ���� ȿ���� ĳ���� ȸ�� Ȯ���� 10%�� ������");
        }

    }

    private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			selectedCharacter.characterData.AttackEffectSpawner.Spawn();
		}

        CheckGoldAndHeal();
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
		
		selectedCharacter.characterData.AttackEffectSpawner = SpawnPoint.GetComponentInChildren<AttackEffectSpawner>();
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        if (selectedCharacter.characterData.IsInvincible)
        {
            Debug.Log($"{selectedCharacter.characterData.CharacterName}��(��) ��ȿȭ �����̹Ƿ� ���� {totalDamage} ��ȿȭ��!");
            return;
        }

        if (selectedCharacter.characterData.NegateNextDamage)
        {
            Debug.Log($"{selectedCharacter.characterData.CharacterName}�� �ñر� ȿ���� ���� {totalDamage} �������� ��ȿȭ�Ǿ����ϴ�.");
            selectedCharacter.characterData.NegateNextDamage = false;
            return;
        }

        float dodgeRoll = Random.Range(0f, 100f);
        if (dodgeRoll < CharacterBaseDodgeChance)
        {
            Debug.Log($"[CharacterManager] {selectedCharacter.characterData.CharacterName}��(��) ������ ȸ���߽��ϴ�! �������� ���� �ʽ��ϴ�.");
            return;
        }

        selectedCharacter.characterData.CurrentHp -= totalDamage;
        savedHp = selectedCharacter.characterData.CurrentHp;
        SaveHp();

        int executionThreshold = Mathf.CeilToInt(selectedCharacter.characterData.MaxHp * (selectedCharacter.characterData.ExecutionRate / 100f));
        if (selectedCharacter.characterData.CurrentHp <= executionThreshold)
        {
            var treasureEffect = Object.FindFirstObjectByType<TreasureEffect>();
            if (treasureEffect != null && treasureEffect.TotemOfResistance && !GameStartTracker.IsUsedTotemOfResistance)
            {
                GameStartTracker.IsUsedTotemOfResistance = true;

                int healAmount = selectedCharacter.characterData.ExecutionRate;
                selectedCharacter.characterData.CurrentHp += healAmount;
                selectedCharacter.characterData.CurrentHp = Mathf.Min(selectedCharacter.characterData.CurrentHp, selectedCharacter.characterData.MaxHp);
                SaveHp();

                Debug.Log($"[TotemOfResistance] ó���� ���� HP {healAmount} ȸ��! ���� HP: {selectedCharacter.characterData.CurrentHp}");
                return;
            }

            Debug.Log("[Execution] ó�� ���� ����. ĳ���� ��� ó��");
            CharacterDied();
            return;
        }

        if (selectedCharacter.characterData.CurrentHp <= 0)
        {
            CharacterDied();
        }
    }



    public void RecoverHpFromDamage(int damageDealt)
    {
        int healAmount = damageDealt / 2;
        selectedCharacter.characterData.CurrentHp += healAmount;

        if (selectedCharacter.characterData.CurrentHp > selectedCharacter.characterData.MaxHp)
            selectedCharacter.characterData.CurrentHp = selectedCharacter.characterData.MaxHp;

        SaveHp();
        Debug.Log($"[CharacterManager] �ñر� ���� ȿ��: {damageDealt} �������� ���� {healAmount} ��ŭ HP ȸ��. ���� HP: {selectedCharacter.characterData.CurrentHp}");
    }


    private void CharacterDied()
    {
        Debug.Log("ĳ���Ͱ� �׾����ϴ�!");
        GameStartTracker.IsHavetobeReset = true;
        scoreScreen.SetActive(true);
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

    public void RecoverHp(int amount)
    {
        selectedCharacter.characterData.CurrentHp += amount;
        if (selectedCharacter.characterData.CurrentHp > selectedCharacter.characterData.MaxHp)
        {
            selectedCharacter.characterData.CurrentHp = selectedCharacter.characterData.MaxHp;
        }
        SaveHp();
        Debug.Log($"[CharacterManager] HP ȸ����: {amount}, ���� HP: {selectedCharacter.characterData.CurrentHp}");
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

    private void CheckGoldAndHeal()
    {
        var treasureEffect = Object.FindFirstObjectByType<TreasureEffect>();
        if (treasureEffect == null || !treasureEffect.BusinessAcumen)
            return;

        int bonusCount = goldData.InGameGold / 30;

        if (bonusCount > lastCheckedHpBonus)
        {
            int healAmount = bonusCount - lastCheckedHpBonus;
            selectedCharacter.characterData.CurrentHp += healAmount;
            lastCheckedHpBonus = bonusCount;
            SaveHp();

            Debug.Log($"[BusinessAcumen] ��� {goldData.InGameGold} �� HP {healAmount} ȸ����!");
        }
    }



}
