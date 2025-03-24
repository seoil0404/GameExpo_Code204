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
            Debug.Log("캐릭터 초기화 실행");
            selectedCharacter.characterData.Initialize();
            ResetHp();
            SaveHp();
            ResetUltimateGauge();
            GameStartTracker.IsHavetobeReset = false;
            if (treasureEffect.Condemnation)
            {
                selectedCharacter.characterData.ExecutionRate += 5;
                Debug.Log("Condemnation 보물 효과로 처형률 +5 증가됨");
            }
        }

        LoadHp();
        LoadUltimateGauge();

        if (currentCharacterInstance == null)
            InitializeCharacter(selectedCharacter);

        if (treasureEffect.MoneyBack && !GameStartTracker.IsUsedMoneyBag)
        {
            GameStartTracker.IsUsedMoneyBag = true;
            goldData.InGameGold += 100;
            Debug.Log("MoneyBag 보물 효과로 골드 100 지급!");
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
            Debug.Log($"[CharacterManager] 기존 캐릭터({currentCharacterInstance.name})가 이미 존재합니다.");
            return;
        }
        currentCharacterInstance = Instantiate(character.characterData.characterPrefab, SpawnPoint);
        RectTransform rectTransform = currentCharacterInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
            rectTransform.localPosition = Vector3.zero;
        Debug.Log($"[CharacterManager] 캐릭터 프리팹 생성: {character.characterData.CharacterName}");
		
		selectedCharacter.characterData.AttackEffectSpawner = SpawnPoint.GetComponentInChildren<AttackEffectSpawner>();
    }

    public void ApplyDamageToCharacter(int totalDamage)
    {
        if (selectedCharacter.characterData.NegateNextDamage)
        {
            Debug.Log($"{selectedCharacter.characterData.CharacterName}의 궁극기 효과로 인해 {totalDamage} 데미지가 무효화되었습니다.");
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


    public void RecoverHpFromDamage(int damageDealt)
    {
        int healAmount = damageDealt / 2;
        selectedCharacter.characterData.CurrentHp += healAmount;

        if (selectedCharacter.characterData.CurrentHp > selectedCharacter.characterData.MaxHp)
            selectedCharacter.characterData.CurrentHp = selectedCharacter.characterData.MaxHp;

        SaveHp();
        Debug.Log($"[CharacterManager] 궁극기 흡혈 효과: {damageDealt} 데미지를 가해 {healAmount} 만큼 HP 회복. 현재 HP: {selectedCharacter.characterData.CurrentHp}");
    }


    private void CharacterDied()
    {
        Debug.Log("캐릭터가 죽었습니다!");
        scoreScreen.SetActive(true);
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

    public void RecoverHp(int amount)
    {
        selectedCharacter.characterData.CurrentHp += amount;
        if (selectedCharacter.characterData.CurrentHp > selectedCharacter.characterData.MaxHp)
        {
            selectedCharacter.characterData.CurrentHp = selectedCharacter.characterData.MaxHp;
        }
        SaveHp();
        Debug.Log($"[CharacterManager] HP 회복됨: {amount}, 현재 HP: {selectedCharacter.characterData.CurrentHp}");
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

            Debug.Log($"[BusinessAcumen] 골드 {goldData.InGameGold} → HP {healAmount} 회복됨!");
        }
    }



}
