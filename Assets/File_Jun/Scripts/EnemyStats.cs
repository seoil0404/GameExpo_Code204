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
            Debug.LogError("CharacterManager를 찾을 수 없습니다!");
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
                return 1; // 기본값
        }
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    public void SetStats(int difficulty, EnemyData.HabitatType habitat)
    {
        int habitatLevel = GetHabitatLevel(habitat);

        // HP 계산: (기본 HP) + (난이도 * 레벨)
        maxHp = enemyData.baseHP + (difficulty * habitatLevel);
        hp = maxHp;

        // ATK 계산: 기본 ATK + (난이도 / (레벨 % 4))
        atk = enemyData.baseATK + (difficulty / Mathf.Max(habitatLevel % 4, 1));

        dodgeChance = enemyData.dodgeChance;

        Debug.Log($"[EnemyStats] {gameObject.name} 스탯 설정 완료 - HP: {hp}, ATK: {atk}, " +
                  $"난이도: {difficulty}, 레벨: {habitatLevel}");

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
            Debug.Log($"[{gameObject.name}]이(가) 스킬 [{chosenSkill.skillName}]을(를) 사용합니다!");
            chosenSkill.ActivateSkill(grid, gameObject);
        }
    }

    public void AttackPlayer()
    {
        if (characterManager == null)
        {
            Debug.LogError("CharacterManager를 찾을 수 없습니다!");
            return;
        }

        int damage = atk;

		if (attackEffectSpawner != null)
		{
			GameObject target = characterManager.SpawnPoint.GetChild(0).gameObject;
			attackEffectSpawner.TargetTransform = target.transform;
			attackEffectSpawner.Spawn(() =>
			{
				Debug.Log($"[{gameObject.name}]이(가) 플레이어를 공격하여 {damage} 데미지를 입힙니다.");
				characterManager.ApplyDamageToCharacter(damage);
			});
		}
		else
		{
  			Debug.Log($"[{gameObject.name}]이(가) 플레이어를 공격하여 {damage} 데미지를 입힙니다.");
			characterManager.ApplyDamageToCharacter(damage);
		}
    }

    public void ReceiveDamage(int completedLines, int gridColumns)
    {
        float dodgeRoll = Random.Range(0, 100);
        Debug.Log($"회피 체크: 랜덤값({dodgeRoll}) vs 회피 확률({dodgeChance}%)");

        if (dodgeRoll < dodgeChance)
        {
            Debug.Log($"[{gameObject.name}]이(가) 공격을 회피했습니다! 데미지를 받지 않습니다.");
            return;
        }

        int totalBlocksUsed = completedLines;
        int baseDamage = totalBlocksUsed;
        int calculatedDamage = baseDamage;

        damageReceivedLastTurn = calculatedDamage;
        hp -= calculatedDamage;

        Debug.Log($"[{gameObject.name}]에게 {calculatedDamage} 데미지를 입혔습니다.");

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
}
