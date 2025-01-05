using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public int currentDifficulty = 1;
    public List<GameObject> enemyPrefabs; 
    public Transform canvasTransform;
    public List<Transform> spawnPoints; 

    public float threeEnemiesChance = 0.2f;
    public float twoEnemiesChance = 0.4f;
    public float oneEnemiesChance = 0.4f;

    private List<Transform> availableSpawnPoints;

    private void Start()
    {
        SpawnEnemies();
    }
    public void SpawnEnemies()
    {
        ResetSpawnPoints();

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
            stats.SetStats(currentDifficulty);
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
}
