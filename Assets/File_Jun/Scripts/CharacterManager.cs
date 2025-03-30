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

    
    public int reflectDamage = 0; //가시 효과



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

        if (treasureEffect.RingOfTime && !GameStartTracker.IsUsedRingofTime)
        {
            GameStartTracker.IsUsedRingofTime = true;

            int gaugeAmount = Mathf.RoundToInt(selectedCharacter.characterData.MaxUltimateGauge * 0.8f);
            selectedCharacter.characterData.MaxUltimateGauge = gaugeAmount;


            Debug.Log($"[RingOfTime] 궁극기 게이지를 최대의 4/5({gaugeAmount})로 설정. 사용 처리됨.");
        }

        if (treasureEffect.NobleBlood)
        {
            int recoverAmount = Mathf.Max(1, selectedCharacter.characterData.CurrentHp / 10);
            selectedCharacter.characterData.CurrentHp += recoverAmount;

            if (selectedCharacter.characterData.CurrentHp > selectedCharacter.characterData.MaxHp)
                selectedCharacter.characterData.CurrentHp = selectedCharacter.characterData.MaxHp;

            SaveHp();
            Debug.Log($"[NobleBlood] 보물 효과로 현재 체력의 1/10({recoverAmount}) 회복됨. 현재 HP: {selectedCharacter.characterData.CurrentHp}");
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

        if (treasureEffect.HolyShield)
        {
            selectedCharacter.characterData.IsInvincible = true;
            EffectManager.Instance.SpawnShield(currentCharacterInstance);
            Debug.Log("[HolyShield] 보물 효과로 무효화 상태가 시작 시 1턴간 적용됩니다.");
        }

        if (treasureEffect.GoldenApple && !GameStartTracker.IsUsedGoldenApple)
        {
            GameStartTracker.IsUsedGoldenApple = true;
            CharacterManager.selectedCharacter.characterData.IncreaseMaxHp(10);
            Debug.Log("황금 사과 효과로 HP10증가");
        }

        if (treasureEffect.ShoesOfHermes)
        {
            CharacterBaseDodgeChance = 10f;
            Debug.Log("ShoesOfHermes 보물 효과로 캐릭터 회피 확률이 10%로 설정됨");
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

    public void ApplyDamageToCharacter(int totalDamage, GameObject attacker = null)
    {
        if (selectedCharacter.characterData.IsInvincible)
        {
            Debug.Log($"{selectedCharacter.characterData.CharacterName}은(는) 무효화 상태이므로 피해 {totalDamage} 무효화됨!");
            SoundManager.Instance.PlayInvincibleSound();
            return;
        }

        if (selectedCharacter.characterData.NegateNextDamage)
        {
            Debug.Log($"{selectedCharacter.characterData.CharacterName}의 궁극기 효과로 인해 {totalDamage} 데미지가 무효화되었습니다.");
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
            Debug.Log($"[CharacterManager] {selectedCharacter.characterData.CharacterName}이(가) 공격을 회피했습니다! 데미지를 받지 않습니다.");
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
                Debug.Log($"[ReflectDamage] {attacker.name}에게 반사 피해 {reflectDamage} 적용됨!");
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
                Debug.Log($"[TotemOfResistance] 처형을 막고 HP {healAmount} 회복! 현재 HP: {selectedCharacter.characterData.CurrentHp}");
                return;
            }

            Debug.Log("[Execution] 처형 조건 충족. 캐릭터 사망 처리");
            CharacterDied();
            return;
        }
        else if (selectedCharacter.characterData.CurrentHp <= executionThreshold)
        {
            isExecutionReady = true;
            Debug.Log($"[Execution] {selectedCharacter.characterData.CharacterName}이(가) 처형 가능 상태에 진입했습니다. 다음 피해 시 처형됩니다.");
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
        Debug.Log($"[CharacterManager] 궁극기 흡혈 효과: {damageDealt} 데미지를 가해 {healAmount} 만큼 HP 회복. 현재 HP: {selectedCharacter.characterData.CurrentHp}");
    }


    private void CharacterDied()
    {
        ScrollManager scrollManager = FindAnyObjectByType<ScrollManager>();
        if (scrollManager != null && scrollManager.OnUseLifeScroll())
        {
            Debug.Log("[Scroll] 생명 스크롤이 사용되어 캐릭터의 죽음을 방지했습니다!");
            return;
        }

        Debug.Log("캐릭터가 죽었습니다!");
        GameStartTracker.IsHavetobeReset = true;
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

    public void HealByPercentageOfMaxHp(float percentage)
    {
        int healAmount = Mathf.RoundToInt(selectedCharacter.characterData.MaxHp * percentage);
        selectedCharacter.characterData.CurrentHp += healAmount;
        selectedCharacter.characterData.CurrentHp = Mathf.Min(selectedCharacter.characterData.CurrentHp, selectedCharacter.characterData.MaxHp);
        SaveHp();

        Debug.Log($"[Heal] 최대 체력의 {percentage * 100}%({healAmount}) 회복됨. 현재 HP: {selectedCharacter.characterData.CurrentHp}");
    }

    public void ApplyDodgeBuff(int turns)
    {
        dodgeBuffTurnsRemaining += turns;
        CharacterBaseDodgeChance = 50f;
        Debug.Log($"[회피 버프] {turns}턴 동안 회피율이 50%로 설정됨");
    }

    public void TickDodgeBuff()
    {
        if (dodgeBuffTurnsRemaining > 0)
        {
            dodgeBuffTurnsRemaining--;
            Debug.Log($"[DodgeBuff] 회피 버프 턴 감소됨. 남은 턴: {dodgeBuffTurnsRemaining}");
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
        Debug.Log($"[ReflectDamage] 다음 피해 시 반사 데미지 {reflectDamage} 적용 예정.");
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

        Debug.Log($"[TakeFixedDamage] {selectedCharacter.characterData.CharacterName}이(가) 고정 피해 {amount}를 입음. 현재 HP: {selectedCharacter.characterData.CurrentHp}");

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
                Debug.Log($"[CharacterManager] {enemy.gameObject.name}에게 고정 데미지 {amount}를 줌.");
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

        Debug.Log($"[ResetHpAndRecoverThirtyPercent] HP를 0으로 만든 뒤, 최대 체력의 30%({recoverAmount})만큼 회복됨.");
    }

}
