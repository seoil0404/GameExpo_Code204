using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    private int atk;
    private float dodgeChance;
    private int comboCount = 0;
    private static List<GameObject> enemies = new List<GameObject>();
    private int damageReceivedLastTurn = 0;

    private int maxHp;

    // ─────────────────────────────────────────────
    // [EatBlock] 스킬 관련 필드
    // ─────────────────────────────────────────────
    private bool hasUsedEatBlock = false;  // EatBlock 스킬을 단 한 번만 사용 가능
    private GameObject eatenBlock = null;  // 먹은 블록(필요 시 적이 죽을 때 삭제 등)

    private void Start()
    {
        characterManager = FindFirstObjectByType<CharacterManager>();

        if (characterManager == null)
        {
            Debug.LogError("CharacterManager를 찾을 수 없습니다!");
        }

        SetStats();
    }

    public int GetHabitatLevel()
    {
        switch (enemyData.habitat)
        {
            case EnemyData.HabitatType.Forest:
                return 1;
            case EnemyData.HabitatType.Castle:
                return 2;
            case EnemyData.HabitatType.DevilCastle:
                return 3;
            default:
                return 0;
        }
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    public void SetStats()
    {
        int currentDifficulty = PlayerPrefs.GetInt("Difficulty", 1);
        int habitatLevel = GetHabitatLevel();

        hp = enemyData.baseHP + (habitatLevel * currentDifficulty);
        maxHp = hp;
        atk = enemyData.baseATK + (currentDifficulty / Mathf.Max(habitatLevel % 4, 1));
        dodgeChance = enemyData.dodgeChance;

        UpdateHealthText();
    }

    // ─────────────────────────────────────────────
    // 적의 턴 액션: 공격 or 스킬 중 하나 선택
    // ─────────────────────────────────────────────
    public void PerformTurnAction(Grid grid)
    {
        // enemyData.enemySkills가 비어 있으면 -> 무조건 공격
        if (enemyData.enemySkills == null || enemyData.enemySkills.Count == 0)
        {
            AttackPlayer();
            return;
        }

        // (스킬 개수 + 1) = N+1 중 하나를 골라서
        // 마지막 인덱스이면 공격, 그 외면 스킬
        int totalOptions = enemyData.enemySkills.Count + 1; // +1 for Attack
        int randomIndex = Random.Range(0, totalOptions);

        if (randomIndex == enemyData.enemySkills.Count)
        {
            // 맨 마지막 인덱스 -> 공격
            AttackPlayer();
        }
        else
        {
            // 0 ~ (N-1) -> 스킬
            EnemySkill chosenSkill = enemyData.enemySkills[randomIndex];
            Debug.Log($"[{gameObject.name}]이(가) 스킬 [{chosenSkill.skillName}]을(를) 사용합니다!");
            chosenSkill.ActivateSkill(grid, gameObject);
        }
    }

    // ─────────────────────────────────────────────
    // 플레이어 공격
    // ─────────────────────────────────────────────
    public void AttackPlayer()
    {
        if (characterManager == null)
        {
            Debug.LogError("CharacterManager를 찾을 수 없습니다!");
            return;
        }

        int damage = atk;
        Debug.Log($"[{gameObject.name}]이(가) 플레이어를 공격하여 {damage} 데미지를 입힙니다.");
        characterManager.ApplyDamageToCharacter(damage);
    }

    public void IncreaseATK()
    {
        int increaseAmount = damageReceivedLastTurn / 2;
        atk += increaseAmount;
        Debug.Log($"[{gameObject.name}]이(가) 지난 턴 피해({damageReceivedLastTurn})의 절반만큼 ATK 증가! 현재 ATK: {atk}");

        // 받은 피해 초기화 (다음 턴을 위해)
        damageReceivedLastTurn = 0;
    }


    // ─────────────────────────────────────────────
    // 라인 클리어 시 적이 받는 데미지 처리
    // ─────────────────────────────────────────────
    public void ReceiveDamage(int completedLines, int gridColumns)
    {
        float dodgeRoll = Random.Range(0, 100);
        Debug.Log($"회피 체크: 랜덤값({dodgeRoll}) vs 회피 확률({dodgeChance}%)");

        if (dodgeRoll < dodgeChance)
        {
            Debug.Log($"[{gameObject.name}]이(가) 공격을 회피했습니다! 데미지를 받지 않습니다.");
            return;
        }

        int totalBlocksUsed = completedLines * gridColumns;
        int baseDamage = totalBlocksUsed;
        int calculatedDamage = baseDamage + (comboCount * 2);

        // 받은 피해 저장
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

        // 적이 죽을 때 eatenBlock을 어떻게 처리할지 결정
        // 예) 이미 필드에서 제거했으므로 별도 처리가 필요없을 수도 있고,
        //     아니면 다른 로직(복원 등)을 구현할 수도 있음
        if (eatenBlock != null)
        {
            // 여기서는 먹은 블록 오브젝트가 존재한다면 Destroy
            Destroy(eatenBlock);
            eatenBlock = null;
        }

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

    private static IEnumerator DelayedSelectRandomEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        EnemySelector.SelectRandomEnemy();
    }

    public static List<GameObject> GetAllEnemies()
    {
        return new List<GameObject>(enemies);
    }

    public int GetAttackDamage()
    {
        return atk;
    }

    public int GetCurrentHp()
    {
        return hp;
    }

    public float GetDodgeChance()
    {
        return dodgeChance;
    }

    // ─────────────────────────────────────────────
    // [EatBlock] 관련 메서드
    // ─────────────────────────────────────────────

    // 한 번만 사용 가능
    public bool HasUsedEatBlock()
    {
        return hasUsedEatBlock;
    }

    // 먹은 블록 참조를 저장하고, 스킬 사용 여부를 true로
    public void SetEatenBlock(GameObject block)
    {
        eatenBlock = block;
        hasUsedEatBlock = true;
        Debug.Log($"{gameObject.name}이(가) EatBlock 스킬을 사용하여 블록을 먹었습니다.");
    }
}
