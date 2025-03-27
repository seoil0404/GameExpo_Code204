using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public EnemyData enemyData;
    public Text healthText;
    public EnemyHealthBar enemyHealthBar;
    private int thornCount = 0;


    [Header("Scriptable")]
    [SerializeField] private GoldData goldData;

    private EnemySpawner spawner;
    private CharacterManager characterManager;
    private int hp;
    private int maxHp;
    private int atk;
    private float dodgeChance;
    private int comboCount = 0;
    private static List<GameObject> enemies = new List<GameObject>();
    private int damageReceivedLastTurn = 0;
	private AttackEffectSpawner attackEffectSpawner;
    private EnemyNextAction enemyNextAction;
    private bool hasUsedSwallowBlock = false;

    private int poisonDamage = 0;
    private int poisonDuration = 0;

    private bool isDamageMultiplierActive = false;

    private bool isSilenced = false;
    private int silenceTurnsRemaining = 0;  

    public void ActivateDamageMultiplier()
    {
        isDamageMultiplierActive = true;
    }

    public void DeactivateDamageMultiplier()
    {
        isDamageMultiplierActive = false;
    }
    public bool HasUsedSwallowBlock => hasUsedSwallowBlock;

    private void Awake() 
	{
		attackEffectSpawner = GetComponentInChildren<AttackEffectSpawner>();
        enemyNextAction = GetComponentInChildren<EnemyNextAction>();
    }

	private void Start()
    {

        characterManager = FindFirstObjectByType<CharacterManager>();

        if (characterManager == null)
        {
            Debug.LogError("CharacterManager를 찾을 수 없습니다!");
        }

        DecideNextAction();
    }

    public int GetHabitatLevel(EnemyData.HabitatType habitat)
    {
        switch (habitat)
        {
            case EnemyData.HabitatType.Forest:
                return 1;
            case EnemyData.HabitatType.Castle:
                return 2;
            case EnemyData.HabitatType.DevilCastle:
                return 3;
            default:
                return 1;
        }
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    public void SetStats(int difficulty, EnemyData.HabitatType habitat)
    {
        Debug.Log($"[SetStats] 함수가 호출되었습니다! {gameObject.name}");
        int habitatLevel = GetHabitatLevel(habitat);

        maxHp = enemyData.baseHP + (difficulty * habitatLevel);
        hp = maxHp;

        atk = enemyData.baseATK + (difficulty / Mathf.Max(4 - habitatLevel, 1));

        dodgeChance = enemyData.dodgeChance;

        var treasureEffect = FindFirstObjectByType<TreasureEffect>();
        if (treasureEffect != null && treasureEffect.UniversalGravitation)
        {
            dodgeChance = 0;
            Debug.Log($"[보물 효과 적용] {gameObject.name}의 회피율이 0으로 설정되었습니다 (UniversalGravitation)");
        }

        Debug.Log($"[EnemyStats] {gameObject.name} 스탯 설정 완료 - HP: {hp}, ATK: {atk}, " +
                  $"난이도: {difficulty}, 레벨: {habitatLevel}");

        UpdateHealthText();
    }

    public void DecideNextAction()
    {
        int totalOptions = enemyData.enemySkills.Count + 1;

        if (isSilenced)
        {
            Debug.Log($"[{gameObject.name}]은(는) 침묵 상태이므로 행동을 결정하지 않습니다.");
            if (enemyNextAction != null)
            {
                enemyNextAction.HideAllActionIndicators();
            }
            silenceTurnsRemaining--;

            if (silenceTurnsRemaining <= 0)
            {
                isSilenced = false;
                Debug.Log($"[{gameObject.name}]의 침묵 상태가 해제되었습니다.");
            }

            return;
        }


        if (hasUsedSwallowBlock)
        {
            totalOptions -= 1;
        }

        if (enemyNextAction != null)
        {
            enemyNextAction.DecideNextAction(totalOptions, atk);
        }
    }

    
    public void SetSwallowBlockUsed()
    {
        hasUsedSwallowBlock = true;
    }


    public int GetCurrentHp()
    {
        return hp;
    }

    public void PerformTurnAction(Grid grid)
    {
        if (enemyNextAction == null)
        {
            Debug.LogError($"[EnemyStats] {gameObject.name}의 EnemyNextAction이 설정되지 않음!");
            return;
        }

        enemyNextAction.HideAllActionIndicators();

        int actionIndex = enemyNextAction.GetNextActionIndex();

        if(actionIndex == 0)
        {
            Debug.LogError("적이 침묵상태여서 공격을 하지 않았습니다");
            return;
        }

        if (actionIndex == 1)
        {
            if (enemyData.defaultAttackType == EnemyData.DefaultAttackType.ThornAttack)
            {
                PerformThornAttack();
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            int skillIndex = actionIndex - 2;
            if (enemyData.enemySkills != null && skillIndex < enemyData.enemySkills.Count)
            {
                EnemySkill chosenSkill = enemyData.enemySkills[skillIndex];
                Debug.Log($"[{gameObject.name}]이(가) 스킬 [{chosenSkill.skillName}]을(를) 사용합니다!");
                chosenSkill.ActivateSkill(grid, gameObject);
            }
        }

        StartCoroutine(DelayedActionCoroutine());

    }


    public void AttackPlayer()
    {
        if (characterManager == null)
        {
            Debug.LogError("CharacterManager를 찾을 수 없습니다!");
            return;
        }

		AttackPlayerInner();

    }

    private void AttackPlayerInner()
    {
        int damage = atk;

        if (isDamageMultiplierActive)
        {
            damage = Mathf.RoundToInt(damage * 1.2f);
            Debug.Log($"[{gameObject.name}]이(가) 1.2배 피해를 가합니다! 최종 데미지: {damage}");
        }

        if (attackEffectSpawner != null)
        {
            GameObject target = characterManager.SpawnPoint.GetChild(0).gameObject;
            attackEffectSpawner.TargetTransform = target.transform;
            attackEffectSpawner.Spawn(() =>
            {
                Debug.Log($"[{gameObject.name}]이(가) 플레이어를 공격하여 {damage} 데미지를 입힙니다.");
                characterManager.ApplyDamageToCharacter(damage);

                if (enemyData.defaultAttackType == EnemyData.DefaultAttackType.LifeSteal)
                {
                    int healAmount = damage / 2;
                    hp += healAmount;
                    if (hp > maxHp) hp = maxHp;
                    UpdateHealthText();
                    Debug.Log($"[{gameObject.name}]이(가) 흡혈 공격으로 {healAmount} HP 회복!");
                }
            });
        }
        else
        {
            Debug.Log($"[{gameObject.name}]이(가) 플레이어를 공격하여 {damage} 데미지를 입힙니다.");
            characterManager.ApplyDamageToCharacter(damage);

            if (enemyData.defaultAttackType == EnemyData.DefaultAttackType.LifeSteal)
            {
                int healAmount = damage / 2;
                hp += healAmount;
                if (hp > maxHp) hp = maxHp;
                UpdateHealthText();
                Debug.Log($"[{gameObject.name}]이(가) 흡혈 공격으로 {healAmount} HP 회복!");
            }
        }
    }



    public void ReceiveDamage(int completedLines, int gridColumns)
    {
        float currentDodgeChance = dodgeChance;
        float dodgeRoll = Random.Range(0, 100);

        if (dodgeRoll < currentDodgeChance)
        {
            EffectManager.Instance.OnMiss(gameObject, CharacterManager.currentCharacterInstance);
            Debug.Log($"[{gameObject.name}]이(가) 공격을 회피했습니다! 데미지를 받지 않습니다.");
            return;
        }

        int baseDamage = completedLines + CharacterManager.selectedCharacter.characterData.CurrentCharacterATK;
        int calculatedDamage = baseDamage;

        var treasureEffect = FindFirstObjectByType<TreasureEffect>();
        if (treasureEffect != null && treasureEffect.SwordOfRuinedKing)
        {
            int bonusDamage = Mathf.CeilToInt(maxHp * 0.08f);
            calculatedDamage += bonusDamage;
            Debug.Log($"[SwordOfRuinedKing] {gameObject.name}에게 보너스 데미지 {bonusDamage} 적용! 총 데미지: {calculatedDamage}");
        }

        if (treasureEffect.GoldenHair && baseDamage >= 32)
        {
            CharacterManager.selectedCharacter.characterData.CurrentHp += calculatedDamage;
           
            goldData.InGameGold += 100;
            CharacterManager.instance.RecoverHpFromDamage(calculatedDamage);


            Debug.Log($"[GoldenHair] baseDamage {baseDamage} ≥ 32 → HP {calculatedDamage} 회복 + 골드 100 획득!");
        }

        damageReceivedLastTurn = calculatedDamage;
        hp -= calculatedDamage;

        Debug.Log($"[{gameObject.name}]에게 {calculatedDamage} 데미지를 입혔습니다.");

        if (thornCount > 0)
        {
            int thornDamage = thornCount;
            Debug.Log($"[{gameObject.name}]의 가시에 의해 플레이어가 {thornDamage} 반사 피해를 입습니다!");
            characterManager.ApplyDamageToCharacter(thornDamage);
        }

        if (CharacterManager.selectedCharacter.characterData.NextAttackLifeSteal)
        {
            CharacterManager.instance.RecoverHpFromDamage(calculatedDamage);
            CharacterManager.selectedCharacter.characterData.NextAttackLifeSteal = false;
        }

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }

        comboCount++;
        UpdateHealthText();
    }



    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{hp}";
        }
        enemyHealthBar.UpdateHpBar();
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name}이(가) 죽었습니다!");

        var treasureEffect = GameObject.FindFirstObjectByType<TreasureEffect>();
        if (treasureEffect != null && treasureEffect.SoulLantern)
        {
            CharacterManager.selectedCharacter.characterData.CurrentCharacterATK += 1;
            Debug.Log("[SoulLantern] 보물 효과: 플레이어 현재 ATK +1!");
        }

        goldData.InGameGold += maxHp;

        if (Grid.instance != null)
        {
            Grid.instance.RemoveEnemy(gameObject);
        }
    }

    public static void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            Debug.Log($"[EnemyStats] 적 추가됨: {enemy.name}, 현재 적 개수: {enemies.Count}");
        }
    }

    public static void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"[EnemyStats] 적 제거됨: {enemy.name}, 남은 적 개수: {enemies.Count}");

            Destroy(enemy);

            if (enemies.Count > 0)
            {
                FindFirstObjectByType<EnemyStats>().StartCoroutine(DelayedSelectRandomEnemy());
            }
            else
            {
                Debug.Log("[EnemyStats] 모든 적이 제거됨! CheckIfGameEnded() 실행");
                FindFirstObjectByType<Grid>().CheckIfGameEnded();
            }
        }
        else
        {
            Debug.LogWarning($"[EnemyStats] {enemy.name}이(가) 리스트에 없음.");
        }
    }

    public void IncreaseATK()
    {
        int increaseAmount = damageReceivedLastTurn / 2;
        atk += increaseAmount;
        Debug.Log($"[{gameObject.name}]이(가) 지난 턴 피해({damageReceivedLastTurn})의 절반만큼 ATK 증가! 현재 ATK: {atk}");

        damageReceivedLastTurn = 0;
    }

    private static IEnumerator DelayedSelectRandomEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        EnemySelector.SelectRandomEnemy();
    }



    public void ApplyPoison(int damage)
    {
        int newPoison = damage / 2;

        if (newPoison <= 0)
            return;

        poisonDamage += newPoison;
        poisonDuration += newPoison;

        Debug.Log($"[{gameObject.name}]이(가) 플레이어에게 독을 적용! 총 피해량: {poisonDamage}, 지속 시간: {poisonDuration}");
    }

    public void ApplyPoisonDamageToPlayer()
    {
        if (poisonDuration > 0)
        {
            characterManager.ApplyDamageToCharacter(poisonDamage);
            Debug.Log($"[플레이어]이(가) {poisonDamage}의 독 피해를 입음. 남은 턴: {poisonDuration - 1}");
            EffectManager.Instance.OnPoison(CharacterManager.currentCharacterInstance);

            poisonDamage--;
            poisonDuration--;

            if (poisonDuration <= 0)
            {
                Debug.Log("[플레이어]의 독 효과가 종료되었습니다.");
            }
        }
    }

    public int GetPoisonDuration()
    {
        return poisonDuration;
    }
    
    public int GetAttack()
    {
        return atk;
    }

    public void IncreaseATKByOne()
    {
        atk += 1;
        Debug.Log($"[{gameObject.name}]의 ATK가 1 증가! 현재 ATK: {atk}");
    }


    public void IncreaseThorn()
    {
        thornCount++;
        Debug.Log($"[{gameObject.name}]의 가시 수치가 1 증가! 현재: {thornCount}");
    }

    public int GetThornCount()
    {
        return thornCount;
    }


    public void PerformThornAttack()
    {
        int thornDamage = Mathf.RoundToInt(atk / 2f);

        if (attackEffectSpawner != null)
        {
            GameObject target = characterManager.SpawnPoint.GetChild(0).gameObject;
            attackEffectSpawner.TargetTransform = target.transform;
            attackEffectSpawner.Spawn(() =>
            {
                Debug.Log($"[{gameObject.name}]이(가) [가시 공격]으로 플레이어에게 {thornDamage} 데미지!");
                characterManager.ApplyDamageToCharacter(thornDamage);
                IncreaseThorn();
            });
        }
        else
        {
            Debug.Log($"[{gameObject.name}]이(가) [가시 공격]으로 플레이어에게 {thornDamage} 데미지!");
            characterManager.ApplyDamageToCharacter(thornDamage);
            IncreaseThorn();
        }
    }


    public void ResetThorn()
    {
        thornCount = 0;
        Debug.Log($"[{gameObject.name}]의 가시 수치 초기화됨 (0으로 설정)");
    }

    public void HealByPercentage(float percentage)
    {
        int healAmount = Mathf.RoundToInt(maxHp * percentage);
        hp += healAmount;
        if (hp > maxHp)
            hp = maxHp;

        UpdateHealthText();
        Debug.Log($"[{gameObject.name}]이(가) {healAmount} 만큼 회복했습니다. 현재 HP: {hp}");
    }

    public void IncreaseATKByTwo()
    {
        atk += 2;
        Debug.Log($"[{gameObject.name}]의 ATK가 2 증가! 현재 ATK: {atk}");
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public void HealByAmount(int amount)
    {
        hp += amount;
        if (hp > maxHp)
            hp = maxHp;

        UpdateHealthText();
        Debug.Log($"[{gameObject.name}]이(가) {amount}만큼 회복했습니다. 현재 HP: {hp}");
    }

    private IEnumerator DelayedActionCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        DecideNextAction();
    }

    public void ApplySilence(int turns = 1)
    {
        isSilenced = true;
        silenceTurnsRemaining = turns;
        Debug.Log($"[{gameObject.name}]이(가) {turns}턴 동안 침묵 상태에 걸렸습니다!");
    }

}
