using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public int currentDifficulty = 1;
    public List<GameObject> enemyPrefabs;
    public List<Transform> spawnPoints;
    public Transform canvasTransform;

    public float threeEnemiesChance = 0.2f;
    public float twoEnemiesChance = 0.4f;
    public float oneEnemiesChance = 0.4f;

    private List<Transform> availableSpawnPoints;
    public List<GameObject> enemies = new List<GameObject>();

    public void Awake()
    {
        LoadDifficulty();
        SpawnEnemies();


        var grid = FindFirstObjectByType<Grid>();
        if (grid != null)
        {
            grid.enemies = enemies;
            Debug.Log("[EnemySpawner] Grid에서 enemies 리스트를 참조하도록 설정 완료!");
        }

        Debug.Log($"[EnemySpawner] 시작 시 적 개수: {enemies.Count}");

        if (enemies.Count > 0)
        {
            StartCoroutine(DelayedSelectRandomEnemy());
        }
    }

    public void SaveDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", currentDifficulty);
        PlayerPrefs.Save();
        Debug.Log($"[EnemySpawner] 난이도 저장됨: {currentDifficulty}");
    }

 
    public void LoadDifficulty()
    {
        currentDifficulty = PlayerPrefs.GetInt("Difficulty", 1);
        Debug.Log($"[EnemySpawner] 저장된 난이도 불러옴: {currentDifficulty}");
    }


    public void IncreaseDifficulty()
    {
        currentDifficulty++;
        SaveDifficulty();
        Debug.Log($"[EnemySpawner] 난이도 증가: {currentDifficulty}");
    }

    public void ResetDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", 0);
        PlayerPrefs.Save();
        currentDifficulty = 0;
        Debug.Log("[EnemySpawner] 난이도 초기화됨: 1");
    }

    public void SpawnEnemies()
    {
        ResetSpawnPoints();
        enemies.Clear();
        Debug.Log("적 생성");

        int numberOfEnemies = DetermineEnemyCount();
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("모든 스폰 포인트가 사용 중입니다. 더 이상 몬스터를 생성할 수 없습니다.");
                break;
            }

            GameObject selectedEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            Transform spawnPoint = GetRandomSpawnPoint();

            GameObject enemyInstance = Instantiate(selectedEnemy, canvasTransform);
            RectTransform rectTransform = enemyInstance.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(spawnPoint.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasTransform as RectTransform,
                    screenPosition,
                    Camera.main,
                    out Vector2 localPoint
                );
                rectTransform.anchoredPosition = localPoint;
            }
            else
            {
                enemyInstance.transform.position = spawnPoint.position;
            }

            EnemyStats stats = enemyInstance.GetComponent<EnemyStats>();
            if (stats != null)
            {
                stats.SetStats();
                stats.SetSpawner(this);
            }
            else
            {
                Debug.LogError($"[EnemySpawner] {enemyInstance.name}에 EnemyStats 컴포넌트가 없습니다!");
            }

            EnemyStats.AddEnemy(enemyInstance);
            enemies.Add(enemyInstance);
            Debug.Log($"[EnemySpawner] 적 추가됨: {enemyInstance.name}, 현재 적 개수: {enemies.Count}");
        }

        if (enemies.Count > 0)
        {
            StartCoroutine(DelayedSelectRandomEnemy());
        }
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

        if (chance <= oneEnemiesChance)
            return 1;
        else if (chance <= oneEnemiesChance + twoEnemiesChance)
            return 2;
        else
            return 3;
    }

    private IEnumerator DelayedSelectRandomEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        EnemySelector.SelectRandomEnemy();
    }
}
