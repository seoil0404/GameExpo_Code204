using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private List<Transform> availableSpawnPoints;
    public List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        LoadDifficulty();

        if (combatData == null) return;

        SpawnEnemies(combatData.HabitatType, combatData.EnemyType);

        var grid = FindFirstObjectByType<Grid>();
        if (grid != null) grid.enemies = enemies;

        if (enemies.Count > 0) StartCoroutine(DelayedSelectRandomEnemy());
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
        currentDifficulty++;
        SaveDifficulty();
    }

    public void ResetDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", 0);
        PlayerPrefs.Save();
        currentDifficulty = 0;
    }

    public void SpawnEnemies(EnemyData.HabitatType selectedHabitat, EnemyData.EnemyType selectedEnemyType)
{
    ResetSpawnPoints();
    enemies.Clear();

    Debug.Log($"[EnemySpawner] 적 생성 시작 - Habitat: {selectedHabitat}, EnemyType: {selectedEnemyType}");

    int numberOfEnemies = DetermineEnemyCount();
    
    List<GameObject> filteredEnemies = enemyPrefabs.FindAll(enemy =>
    {
        EnemyStats stats = enemy.GetComponent<EnemyStats>();

        if (stats == null || stats.enemyData == null)
        {
            Debug.LogWarning($"[EnemySpawner] {enemy.name}에 EnemyStats 또는 EnemyData가 없습니다!");
            return false;
        }

        bool matches = stats.enemyData.habitat == selectedHabitat &&
                       stats.enemyData.enemyType == selectedEnemyType;

        Debug.Log($"[EnemySpawner] 검사 중 - {enemy.name}: Habitat = {stats.enemyData.habitat}, EnemyType = {stats.enemyData.enemyType}, 매칭 여부: {matches}");

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
        Transform spawnPoint = GetRandomSpawnPoint();
        GameObject enemyInstance = Instantiate(selectedEnemy, spawnPoint.position, Quaternion.identity, canvasTransform);

        Debug.Log($"[EnemySpawner] 적 생성됨: {enemyInstance.name} 위치: {spawnPoint.position}");

        EnemyStats stats = enemyInstance.GetComponent<EnemyStats>();
        if (stats != null)
        {
            stats.SetStats();
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
