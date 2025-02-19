using UnityEngine;
using System.Collections;
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
    public List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        SpawnEnemies();


        var grid = FindFirstObjectByType<Grid>();
        if (grid != null)
        {
            grid.enemies = enemies;
            Debug.Log("[EnemySpawner] Grid���� enemies ����Ʈ�� �����ϵ��� ���� �Ϸ�!");
        }

        Debug.Log($"[EnemySpawner] ���� �� �� ����: {enemies.Count}");

        StartCoroutine(DelayedSelectRandomEnemy());
    }

    public void SpawnEnemies()
    {
        ResetSpawnPoints();
        enemies.Clear(); 

        int numberOfEnemies = DetermineEnemyCount();
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("��� ���� ����Ʈ�� ��� ���Դϴ�. �� �̻� ���͸� ������ �� �����ϴ�.");
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
            stats.SetSpawner(this);

            enemies.Add(enemyInstance);
            Debug.Log($"[EnemySpawner] �� �߰���: {enemyInstance.name}, ���� �� ����: {enemies.Count}");
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

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"[EnemySpawner] �� ���ŵ�: {enemy.name}, ���� �� ����: {enemies.Count}");


            if (enemies.Count > 0)
            {
                StartCoroutine(DelayedSelectRandomEnemy());
            }
            else
            {
                Debug.Log("[EnemySpawner] ��� ���� ���ŵ�! CheckIfGameEnded() ����");
                FindFirstObjectByType<Grid>().CheckIfGameEnded();
            }
        }
        else
        {
            Debug.LogWarning($"[EnemySpawner] {enemy.name}��(��) ����Ʈ�� ����.");
        }
    }

    private IEnumerator DelayedSelectRandomEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        EnemySelector.SelectRandomEnemy();
    }
}
