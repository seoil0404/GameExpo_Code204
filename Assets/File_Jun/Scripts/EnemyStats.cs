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
            Debug.LogError("CharacterManager�� ã�� �� �����ϴ�!");
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
        Debug.Log($"[SetStats] �Լ��� ȣ��Ǿ����ϴ�! {gameObject.name}");
        int habitatLevel = GetHabitatLevel(habitat);

        maxHp = enemyData.baseHP + (difficulty * habitatLevel);
        hp = maxHp;

        atk = enemyData.baseATK + (difficulty / Mathf.Max(4 - habitatLevel, 1));

        dodgeChance = enemyData.dodgeChance;

        var treasureEffect = FindFirstObjectByType<TreasureEffect>();
        if (treasureEffect != null && treasureEffect.UniversalGravitation)
        {
            dodgeChance = 0;
            Debug.Log($"[���� ȿ�� ����] {gameObject.name}�� ȸ������ 0���� �����Ǿ����ϴ� (UniversalGravitation)");
        }

        Debug.Log($"[EnemyStats] {gameObject.name} ���� ���� �Ϸ� - HP: {hp}, ATK: {atk}, " +
                  $"���̵�: {difficulty}, ����: {habitatLevel}");

        UpdateHealthText();
    }

    public void DecideNextAction()
    {
        int totalOptions = enemyData.enemySkills.Count + 1;

        if (isSilenced)
        {
            Debug.Log($"[{gameObject.name}]��(��) ħ�� �����̹Ƿ� �ൿ�� �������� �ʽ��ϴ�.");
            if (enemyNextAction != null)
            {
                enemyNextAction.HideAllActionIndicators();
            }
            silenceTurnsRemaining--;

            if (silenceTurnsRemaining <= 0)
            {
                isSilenced = false;
                Debug.Log($"[{gameObject.name}]�� ħ�� ���°� �����Ǿ����ϴ�.");
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
            Debug.LogError($"[EnemyStats] {gameObject.name}�� EnemyNextAction�� �������� ����!");
            return;
        }

        enemyNextAction.HideAllActionIndicators();

        int actionIndex = enemyNextAction.GetNextActionIndex();

        if(actionIndex == 0)
        {
            Debug.LogError("���� ħ�����¿��� ������ ���� �ʾҽ��ϴ�");
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
                Debug.Log($"[{gameObject.name}]��(��) ��ų [{chosenSkill.skillName}]��(��) ����մϴ�!");
                chosenSkill.ActivateSkill(grid, gameObject);
            }
        }

        StartCoroutine(DelayedActionCoroutine());

    }


    public void AttackPlayer()
    {
        if (characterManager == null)
        {
            Debug.LogError("CharacterManager�� ã�� �� �����ϴ�!");
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
            Debug.Log($"[{gameObject.name}]��(��) 1.2�� ���ظ� ���մϴ�! ���� ������: {damage}");
        }

        if (attackEffectSpawner != null)
        {
            GameObject target = characterManager.SpawnPoint.GetChild(0).gameObject;
            attackEffectSpawner.TargetTransform = target.transform;
            attackEffectSpawner.Spawn(() =>
            {
                Debug.Log($"[{gameObject.name}]��(��) �÷��̾ �����Ͽ� {damage} �������� �����ϴ�.");
                characterManager.ApplyDamageToCharacter(damage);

                if (enemyData.defaultAttackType == EnemyData.DefaultAttackType.LifeSteal)
                {
                    int healAmount = damage / 2;
                    hp += healAmount;
                    if (hp > maxHp) hp = maxHp;
                    UpdateHealthText();
                    Debug.Log($"[{gameObject.name}]��(��) ���� �������� {healAmount} HP ȸ��!");
                }
            });
        }
        else
        {
            Debug.Log($"[{gameObject.name}]��(��) �÷��̾ �����Ͽ� {damage} �������� �����ϴ�.");
            characterManager.ApplyDamageToCharacter(damage);

            if (enemyData.defaultAttackType == EnemyData.DefaultAttackType.LifeSteal)
            {
                int healAmount = damage / 2;
                hp += healAmount;
                if (hp > maxHp) hp = maxHp;
                UpdateHealthText();
                Debug.Log($"[{gameObject.name}]��(��) ���� �������� {healAmount} HP ȸ��!");
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
            Debug.Log($"[{gameObject.name}]��(��) ������ ȸ���߽��ϴ�! �������� ���� �ʽ��ϴ�.");
            return;
        }

        int baseDamage = completedLines + CharacterManager.selectedCharacter.characterData.CurrentCharacterATK;
        int calculatedDamage = baseDamage;

        var treasureEffect = FindFirstObjectByType<TreasureEffect>();
        if (treasureEffect != null && treasureEffect.SwordOfRuinedKing)
        {
            int bonusDamage = Mathf.CeilToInt(maxHp * 0.08f);
            calculatedDamage += bonusDamage;
            Debug.Log($"[SwordOfRuinedKing] {gameObject.name}���� ���ʽ� ������ {bonusDamage} ����! �� ������: {calculatedDamage}");
        }

        if (treasureEffect.GoldenHair && baseDamage >= 32)
        {
            CharacterManager.selectedCharacter.characterData.CurrentHp += calculatedDamage;
           
            goldData.InGameGold += 100;
            CharacterManager.instance.RecoverHpFromDamage(calculatedDamage);


            Debug.Log($"[GoldenHair] baseDamage {baseDamage} �� 32 �� HP {calculatedDamage} ȸ�� + ��� 100 ȹ��!");
        }

        damageReceivedLastTurn = calculatedDamage;
        hp -= calculatedDamage;

        Debug.Log($"[{gameObject.name}]���� {calculatedDamage} �������� �������ϴ�.");

        if (thornCount > 0)
        {
            int thornDamage = thornCount;
            Debug.Log($"[{gameObject.name}]�� ���ÿ� ���� �÷��̾ {thornDamage} �ݻ� ���ظ� �Խ��ϴ�!");
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
        Debug.Log($"{gameObject.name}��(��) �׾����ϴ�!");

        var treasureEffect = GameObject.FindFirstObjectByType<TreasureEffect>();
        if (treasureEffect != null && treasureEffect.SoulLantern)
        {
            CharacterManager.selectedCharacter.characterData.CurrentCharacterATK += 1;
            Debug.Log("[SoulLantern] ���� ȿ��: �÷��̾� ���� ATK +1!");
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
            Debug.Log($"[EnemyStats] �� �߰���: {enemy.name}, ���� �� ����: {enemies.Count}");
        }
    }

    public static void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"[EnemyStats] �� ���ŵ�: {enemy.name}, ���� �� ����: {enemies.Count}");

            Destroy(enemy);

            if (enemies.Count > 0)
            {
                FindFirstObjectByType<EnemyStats>().StartCoroutine(DelayedSelectRandomEnemy());
            }
            else
            {
                Debug.Log("[EnemyStats] ��� ���� ���ŵ�! CheckIfGameEnded() ����");
                FindFirstObjectByType<Grid>().CheckIfGameEnded();
            }
        }
        else
        {
            Debug.LogWarning($"[EnemyStats] {enemy.name}��(��) ����Ʈ�� ����.");
        }
    }

    public void IncreaseATK()
    {
        int increaseAmount = damageReceivedLastTurn / 2;
        atk += increaseAmount;
        Debug.Log($"[{gameObject.name}]��(��) ���� �� ����({damageReceivedLastTurn})�� ���ݸ�ŭ ATK ����! ���� ATK: {atk}");

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

        Debug.Log($"[{gameObject.name}]��(��) �÷��̾�� ���� ����! �� ���ط�: {poisonDamage}, ���� �ð�: {poisonDuration}");
    }

    public void ApplyPoisonDamageToPlayer()
    {
        if (poisonDuration > 0)
        {
            characterManager.ApplyDamageToCharacter(poisonDamage);
            Debug.Log($"[�÷��̾�]��(��) {poisonDamage}�� �� ���ظ� ����. ���� ��: {poisonDuration - 1}");
            EffectManager.Instance.OnPoison(CharacterManager.currentCharacterInstance);

            poisonDamage--;
            poisonDuration--;

            if (poisonDuration <= 0)
            {
                Debug.Log("[�÷��̾�]�� �� ȿ���� ����Ǿ����ϴ�.");
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
        Debug.Log($"[{gameObject.name}]�� ATK�� 1 ����! ���� ATK: {atk}");
    }


    public void IncreaseThorn()
    {
        thornCount++;
        Debug.Log($"[{gameObject.name}]�� ���� ��ġ�� 1 ����! ����: {thornCount}");
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
                Debug.Log($"[{gameObject.name}]��(��) [���� ����]���� �÷��̾�� {thornDamage} ������!");
                characterManager.ApplyDamageToCharacter(thornDamage);
                IncreaseThorn();
            });
        }
        else
        {
            Debug.Log($"[{gameObject.name}]��(��) [���� ����]���� �÷��̾�� {thornDamage} ������!");
            characterManager.ApplyDamageToCharacter(thornDamage);
            IncreaseThorn();
        }
    }


    public void ResetThorn()
    {
        thornCount = 0;
        Debug.Log($"[{gameObject.name}]�� ���� ��ġ �ʱ�ȭ�� (0���� ����)");
    }

    public void HealByPercentage(float percentage)
    {
        int healAmount = Mathf.RoundToInt(maxHp * percentage);
        hp += healAmount;
        if (hp > maxHp)
            hp = maxHp;

        UpdateHealthText();
        Debug.Log($"[{gameObject.name}]��(��) {healAmount} ��ŭ ȸ���߽��ϴ�. ���� HP: {hp}");
    }

    public void IncreaseATKByTwo()
    {
        atk += 2;
        Debug.Log($"[{gameObject.name}]�� ATK�� 2 ����! ���� ATK: {atk}");
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
        Debug.Log($"[{gameObject.name}]��(��) {amount}��ŭ ȸ���߽��ϴ�. ���� HP: {hp}");
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
        Debug.Log($"[{gameObject.name}]��(��) {turns}�� ���� ħ�� ���¿� �ɷȽ��ϴ�!");
    }

}
