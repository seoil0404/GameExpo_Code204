using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    public int currentDifficulty = 1;
    public List<GameObject> enemyPrefabs;
    public List<Transform> spawnPoints;
    public Transform canvasTransform;
    public CombatData combatData;

    public float threeEnemiesChance = 0.2f;
    public float twoEnemiesChance = 0.4f;
    public float oneEnemiesChance = 0.4f;

    bool Specialcombating = false;

    private List<Transform> availableSpawnPoints;
    public List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        if (GameStartTracker.IsHavetobeReset)
        {
            ResetDifficulty();
        }
        else
        {
            LoadDifficulty();
            
        }

        if (combatData == null)
        {
            Debug.LogError("[EnemySpawner] CombatData가 설정되지 않았습니다! Inspector에서 CombatData를 할당하세요.");
            return;
        }

        if (combatData.EnemyType == EnemyData.EnemyType.SpecialCombat)
        {
            Debug.LogWarning("[EnemySpawner] SpecialCombat 감지됨! Common으로 변경 후 난이도 10배 증가 및 한 마리만 생성");
            Specialcombating = true;
            combatData.EnemyType = EnemyData.EnemyType.Common;
            currentDifficulty *= 3;

            SpawnEnemies(combatData.HabitatType, combatData.EnemyType, forceOneEnemy: true);
        }
        else if(combatData.EnemyType == EnemyData.EnemyType.Boss)
        {
            Debug.LogWarning("[EnemySpawner] Boss 감지됨 적 하나만 생성");
            SpawnEnemies(combatData.HabitatType, combatData.EnemyType, forceOneEnemy: true);
        }
        else
        {
            SpawnEnemies(combatData.HabitatType, combatData.EnemyType);
        }

        var grid = FindFirstObjectByType<Grid>();
        if (grid != null) grid.enemies = enemies;

        if (enemies.Count > 0)
        {
            Debug.Log("[EnemySpawner] 적 선택 코루틴 시작");
            StartCoroutine(DelayedSelectRandomEnemy());
        }
        else
        {
            Debug.LogWarning("[EnemySpawner] 스폰된 적이 없습니다. 필터링 과정 확인 필요");
        }
    }


    public void SaveDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", currentDifficulty);
        PlayerPrefs.Save();
    }

    public void LoadDifficulty()
    {
        currentDifficulty = PlayerPrefs.GetInt("Difficulty", 1);
    }

    public void IncreaseDifficulty()
    {
        if (Specialcombating)
        {
            currentDifficulty = currentDifficulty / 2 + 2;
        }
        else if (combatData.EnemyType == EnemyData.EnemyType.Boss)
        {
            currentDifficulty += 3;
        }
        else
        {
            currentDifficulty++;
        }

        SaveDifficulty();
    }

    public void ResetDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", 0);
        PlayerPrefs.Save();
        currentDifficulty = 0;
    }

    public void SpawnEnemies(EnemyData.HabitatType selectedHabitat, EnemyData.EnemyType selectedEnemyType, bool forceOneEnemy = false)
    {
        ResetSpawnPoints();
        enemies.Clear();

        int numberOfEnemies = forceOneEnemy ? 1 : DetermineEnemyCount();

        List<GameObject> filteredEnemies = enemyPrefabs.FindAll(enemy =>
        {
            EnemyStats stats = enemy.GetComponent<EnemyStats>();

            if (stats == null || stats.enemyData == null)
            {
                return false;
            }

            bool matches = stats.enemyData.habitat == selectedHabitat &&
                           stats.enemyData.enemyType == selectedEnemyType;

            return matches;
        });

        Debug.Log($"[EnemySpawner] 필터링된 적 개수: {filteredEnemies.Count}");

        if (filteredEnemies.Count == 0)
        {
            Debug.LogWarning("[EnemySpawner] 조건에 맞는 적이 없습니다! 적 생성 중단");
            return;
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("[EnemySpawner] 모든 스폰 포인트가 사용 중입니다. 적 생성 중단");
                break;
            }

            GameObject selectedEnemy = filteredEnemies[Random.Range(0, filteredEnemies.Count)];

        
            Transform spawnPoint;
            if (selectedEnemyType == EnemyData.EnemyType.Boss && spawnPoints.Count > 1)
            {
                spawnPoint = spawnPoints[1];
                Debug.Log("[EnemySpawner] BOSS 스테이지 감지됨! 두 번째 스폰 포인트에서 스폰");
            }
            else
            {
                spawnPoint = GetRandomSpawnPoint();
            }

            GameObject enemyInstance = Instantiate(selectedEnemy, spawnPoint.position, Quaternion.identity, canvasTransform);

            Debug.Log($"[EnemySpawner] 적 생성됨: {enemyInstance.name} 위치: {spawnPoint.position}");

            EnemyStats stats = enemyInstance.GetComponent<EnemyStats>();
            if (stats != null)
            {
                stats.SetStats(currentDifficulty, combatData.HabitatType);
                stats.SetSpawner(this);
            }

            EnemyStats.AddEnemy(enemyInstance);
            enemies.Add(enemyInstance);
        }

        if (enemies.Count > 0) StartCoroutine(DelayedSelectRandomEnemy());
    }


    private void ResetSpawnPoints()
    {
        availableSpawnPoints = new List<Transform>(spawnPoints);
    }

    private Transform GetRandomSpawnPoint()
    {
        int index = Random.Range(0, availableSpawnPoints.Count);
        Transform selectedPoint = availableSpawnPoints[index];
        availableSpawnPoints.RemoveAt(index);
        return selectedPoint;
    }

    private int DetermineEnemyCount()
    {
        float chance = Random.value;
        if (chance <= oneEnemiesChance) return 1;
        else if (chance <= oneEnemiesChance + twoEnemiesChance) return 2;
        else return 3;
    }

    private IEnumerator DelayedSelectRandomEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        EnemySelector.SelectRandomEnemy();
    }
}
