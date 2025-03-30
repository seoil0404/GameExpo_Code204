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
    private bool isExecutionReady = false;
    private int dodgeBuffTurnsRemaining = 0;

    
    public int reflectDamage = 0; //���� ȿ��



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

        if (treasureEffect.RingOfTime && !GameStartTracker.IsUsedRingofTime)
        {
            GameStartTracker.IsUsedRingofTime = true;

            int gaugeAmount = Mathf.RoundToInt(selectedCharacter.characterData.MaxUltimateGauge * 0.8f);
            selectedCharacter.characterData.MaxUltimateGauge = gaugeAmount;


            Debug.Log($"[RingOfTime] �ñر� �������� �ִ��� 4/5({gaugeAmount})�� ����. ��� ó����.");
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

        if (treasureEffect.HolyShield)
        {
            selectedCharacter.characterData.IsInvincible = true;
            EffectManager.Instance.SpawnShield(currentCharacterInstance);
            Debug.Log("[HolyShield] ���� ȿ���� ��ȿȭ ���°� ���� �� 1�ϰ� ����˴ϴ�.");
        }

        if (treasureEffect.GoldenApple && !GameStartTracker.IsUsedGoldenApple)
        {
            GameStartTracker.IsUsedGoldenApple = true;
            CharacterManager.selectedCharacter.characterData.IncreaseMaxHp(10);
            Debug.Log("Ȳ�� ��� ȿ���� HP10����");
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

    public void ApplyDamageToCharacter(int totalDamage, GameObject attacker = null)
    {
        if (selectedCharacter.characterData.IsInvincible)
        {
            Debug.Log($"{selectedCharacter.characterData.CharacterName}��(��) ��ȿȭ �����̹Ƿ� ���� {totalDamage} ��ȿȭ��!");
            SoundManager.Instance.PlayInvincibleSound();
            return;
        }

        if (selectedCharacter.characterData.NegateNextDamage)
        {
            Debug.Log($"{selectedCharacter.characterData.CharacterName}�� �ñر� ȿ���� ���� {totalDamage} �������� ��ȿȭ�Ǿ����ϴ�.");
            selectedCharacter.characterData.NegateNextDamage = false;
            return;
        }

        float effectiveDodgeChance = CharacterBaseDodgeChance;

        if (dodgeBuffTurnsRemaining > 0)
        {
            effectiveDodgeChance = 50f;
        }

        float dodgeRoll = Random.Range(0f, 100f);
        if (dodgeRoll < effectiveDodgeChance)
        {
            Debug.Log($"[CharacterManager] {selectedCharacter.characterData.CharacterName}��(��) ������ ȸ���߽��ϴ�! �������� ���� �ʽ��ϴ�.");
            SoundManager.Instance.PlayDodgeSound();
            EffectManager.Instance.OnMiss(currentCharacterInstance , currentCharacterInstance);
            return;
        }   

        selectedCharacter.characterData.CurrentHp -= totalDamage;
        savedHp = selectedCharacter.characterData.CurrentHp;
        SaveHp();

        if (reflectDamage > 0 && attacker != null)
        {
            EnemyStats enemyStats = attacker.GetComponent<EnemyStats>();
            if (enemyStats != null && enemyStats.GetCurrentHp() > 0)
            {
                enemyStats.TakeFixedDamage(reflectDamage);
                Debug.Log($"[ReflectDamage] {attacker.name}���� �ݻ� ���� {reflectDamage} �����!");
            }
        }

        if (selectedCharacter.characterData.CurrentHp <= 0)
        {
            CharacterDied();
            return;
        }
        int executionThreshold = Mathf.CeilToInt(selectedCharacter.characterData.MaxHp * (selectedCharacter.characterData.ExecutionRate / 100f));

        if (isExecutionReady)
        {
            var treasureEffect = Object.FindFirstObjectByType<TreasureEffect>();
            if (treasureEffect != null && treasureEffect.TotemOfResistance && !GameStartTracker.IsUsedTotemOfResistance)
            {
                GameStartTracker.IsUsedTotemOfResistance = true;

                int healAmount = selectedCharacter.characterData.ExecutionRate;
                selectedCharacter.characterData.CurrentHp += healAmount;
                selectedCharacter.characterData.CurrentHp = Mathf.Min(selectedCharacter.characterData.CurrentHp, selectedCharacter.characterData.MaxHp);
                SaveHp();

                isExecutionReady = false;
                Debug.Log($"[TotemOfResistance] ó���� ���� HP {healAmount} ȸ��! ���� HP: {selectedCharacter.characterData.CurrentHp}");
                return;
            }

            Debug.Log("[Execution] ó�� ���� ����. ĳ���� ��� ó��");
            CharacterDied();
            return;
        }
        else if (selectedCharacter.characterData.CurrentHp <= executionThreshold)
        {
            isExecutionReady = true;
            Debug.Log($"[Execution] {selectedCharacter.characterData.CharacterName}��(��) ó�� ���� ���¿� �����߽��ϴ�. ���� ���� �� ó���˴ϴ�.");
        }
        else
        {
            isExecutionReady = false;
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
        ScrollManager scrollManager = FindAnyObjectByType<ScrollManager>();
        if (scrollManager != null && scrollManager.OnUseLifeScroll())
        {
            Debug.Log("[Scroll] ���� ��ũ���� ���Ǿ� ĳ������ ������ �����߽��ϴ�!");
            return;
        }

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

    public void HealByPercentageOfMaxHp(float percentage)
    {
        int healAmount = Mathf.RoundToInt(selectedCharacter.characterData.MaxHp * percentage);
        selectedCharacter.characterData.CurrentHp += healAmount;
        selectedCharacter.characterData.CurrentHp = Mathf.Min(selectedCharacter.characterData.CurrentHp, selectedCharacter.characterData.MaxHp);
        SaveHp();

        Debug.Log($"[Heal] �ִ� ü���� {percentage * 100}%({healAmount}) ȸ����. ���� HP: {selectedCharacter.characterData.CurrentHp}");
    }

    public void ApplyDodgeBuff(int turns)
    {
        dodgeBuffTurnsRemaining += turns;
        CharacterBaseDodgeChance = 50f;
        Debug.Log($"[ȸ�� ����] {turns}�� ���� ȸ������ 50%�� ������");
    }

    public void TickDodgeBuff()
    {
        if (dodgeBuffTurnsRemaining > 0)
        {
            dodgeBuffTurnsRemaining--;
            Debug.Log($"[DodgeBuff] ȸ�� ���� �� ���ҵ�. ���� ��: {dodgeBuffTurnsRemaining}");
        }
    }

    public void GetGold(int Amount)
    {
        goldData.InGameGold += Amount;  
    }

    public void SetReflectDamage(int amount)
    {
        reflectDamage += amount;
        SoundManager.Instance.PlayBuffSound();
        EffectManager.Instance.OnBuff(currentCharacterInstance , Color.yellow);
        Debug.Log($"[ReflectDamage] ���� ���� �� �ݻ� ������ {reflectDamage} ���� ����.");
    }

    public void ClearReflectDamage()
    {
        reflectDamage = 0;
    }

    public void TakeFixedDamageToCharacter(int amount)
    {
        if (amount <= 0) return;

        selectedCharacter.characterData.CurrentHp -= amount;
        savedHp = selectedCharacter.characterData.CurrentHp;
        SaveHp();

        Debug.Log($"[TakeFixedDamage] {selectedCharacter.characterData.CharacterName}��(��) ���� ���� {amount}�� ����. ���� HP: {selectedCharacter.characterData.CurrentHp}");

        if (selectedCharacter.characterData.CurrentHp <= 0)
        {
            CharacterDied();
        }
    }

    public void DamageAllEnemies(int amount)
    {
        EnemyStats[] enemies = FindObjectsByType<EnemyStats>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            if (enemy != null && enemy.GetCurrentHp() > 0)
            {
                enemy.TakeFixedDamage(amount);
                Debug.Log($"[CharacterManager] {enemy.gameObject.name}���� ���� ������ {amount}�� ��.");
            }   
        }
    }

    public void ResetHpAndRecoverThirtyPercent()
    {
        selectedCharacter.characterData.CurrentHp = 0;
        int recoverAmount = Mathf.RoundToInt(selectedCharacter.characterData.MaxHp * 0.3f);
        selectedCharacter.characterData.CurrentHp = recoverAmount;
        savedHp = recoverAmount;
        SaveHp();

        Debug.Log($"[ResetHpAndRecoverThirtyPercent] HP�� 0���� ���� ��, �ִ� ü���� 30%({recoverAmount})��ŭ ȸ����.");
    }

}
