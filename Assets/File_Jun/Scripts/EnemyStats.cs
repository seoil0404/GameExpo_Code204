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

	private void Awake() 
	{
		attackEffectSpawner = GetComponentInChildren<AttackEffectSpawner>();
	}

	private void Start()
    {
        characterManager = FindFirstObjectByType<CharacterManager>();

        if (characterManager == null)
        {
            Debug.LogError("CharacterManager�� ã�� �� �����ϴ�!");
        }
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
                return 1; // �⺻��
        }
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    public void SetStats(int difficulty, EnemyData.HabitatType habitat)
    {
        int habitatLevel = GetHabitatLevel(habitat);

        // HP ���: (�⺻ HP) + (���̵� * ����)
        maxHp = enemyData.baseHP + (difficulty * habitatLevel);
        hp = maxHp;

        // ATK ���: �⺻ ATK + (���̵� / (���� % 4))
        atk = enemyData.baseATK + (difficulty / Mathf.Max(habitatLevel % 4, 1));

        dodgeChance = enemyData.dodgeChance;

        Debug.Log($"[EnemyStats] {gameObject.name} ���� ���� �Ϸ� - HP: {hp}, ATK: {atk}, " +
                  $"���̵�: {difficulty}, ����: {habitatLevel}");

        UpdateHealthText();
    }

    public int GetCurrentHp()
    {
        return hp;
    }

    public void PerformTurnAction(Grid grid)
    {
        if (enemyData.enemySkills == null || enemyData.enemySkills.Count == 0)
        {
            AttackPlayer();
            return;
        }

        int totalOptions = enemyData.enemySkills.Count + 1;
        int randomIndex = Random.Range(0, totalOptions);

        if (randomIndex == enemyData.enemySkills.Count)
        {
            AttackPlayer();
        }
        else
        {
            EnemySkill chosenSkill = enemyData.enemySkills[randomIndex];
            Debug.Log($"[{gameObject.name}]��(��) ��ų [{chosenSkill.skillName}]��(��) ����մϴ�!");
            chosenSkill.ActivateSkill(grid, gameObject);
        }
    }

    public void AttackPlayer()
    {
        if (characterManager == null)
        {
            Debug.LogError("CharacterManager�� ã�� �� �����ϴ�!");
            return;
        }

        int damage = atk;

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
        float dodgeRoll = Random.Range(0, 100);
        Debug.Log($"ȸ�� üũ: ������({dodgeRoll}) vs ȸ�� Ȯ��({dodgeChance}%)");

        if (dodgeRoll < dodgeChance)
        {
            Debug.Log($"[{gameObject.name}]��(��) ������ ȸ���߽��ϴ�! �������� ���� �ʽ��ϴ�.");
            return;
        }

        int totalBlocksUsed = completedLines;
        int baseDamage = totalBlocksUsed;
        int calculatedDamage = baseDamage;

        damageReceivedLastTurn = calculatedDamage;
        hp -= calculatedDamage;

        Debug.Log($"[{gameObject.name}]���� {calculatedDamage} �������� �������ϴ�.");

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

        Destroy(gameObject);
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
}
