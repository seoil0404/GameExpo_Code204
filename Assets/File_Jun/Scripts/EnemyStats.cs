using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public EnemyData enemyData;
    public Text healthText;
    public EnemyHealthBar enemyHealthBar;



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
        int habitatLevel = GetHabitatLevel(habitat);

        maxHp = enemyData.baseHP + (difficulty * habitatLevel);
        hp = maxHp;

        atk = enemyData.baseATK + (difficulty / Mathf.Max(4 - habitatLevel, 1));

        dodgeChance = enemyData.dodgeChance;

        Debug.Log($"[EnemyStats] {gameObject.name} ���� ���� �Ϸ� - HP: {hp}, ATK: {atk}, " +
                  $"���̵�: {difficulty}, ����: {habitatLevel}");

        UpdateHealthText();
    }

    public void DecideNextAction()
    {
        int totalOptions = enemyData.enemySkills.Count + 1;


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
        int actionIndex = enemyNextAction.GetNextActionIndex();

        if (actionIndex == 1)
        {
            AttackPlayer();
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

        DecideNextAction();
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
            });
        }
        else
        {
            Debug.Log($"[{gameObject.name}]��(��) �÷��̾ �����Ͽ� {damage} �������� �����ϴ�.");
            characterManager.ApplyDamageToCharacter(damage);
        }
    }



    public void ReceiveDamage(int completedLines, int gridColumns)
    {

        float currentDodgeChance = TreasureEffect.IsTreasureActive(TreasureEffect.TreasureType.UniversalGravitation) ? 0f : dodgeChance;

        float dodgeRoll = Random.Range(0, 100);
        Debug.Log($"ȸ�� üũ: ������({dodgeRoll}) vs ȸ�� Ȯ��({currentDodgeChance}%)");

        if (dodgeRoll < currentDodgeChance)
        {
			HitEffectManager.Instance.OnMiss(gameObject, CharacterManager.currentCharacterInstance);
            Debug.Log($"[{gameObject.name}]��(��) ������ ȸ���߽��ϴ�! �������� ���� �ʽ��ϴ�.");
            return;
        }

        int totalBlocksUsed = completedLines;
        int baseDamage = totalBlocksUsed;
        int calculatedDamage = baseDamage;

        damageReceivedLastTurn = calculatedDamage;
        hp -= calculatedDamage;

        Debug.Log($"[{gameObject.name}]���� {calculatedDamage} �������� �������ϴ�.");

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
            HitEffectManager.Instance.OnPoison(CharacterManager.currentCharacterInstance);

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


}
