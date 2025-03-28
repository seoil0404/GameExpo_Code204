using UnityEngine;
using System.Collections.Generic;

public class MinoEffectPool : MonoBehaviour
{
    public GameObject[] minoPrefabs;
    public int initialPoolSize = 12;
    public float shrinkCheckInterval = 3f;
    public int shrinkThreshold = 10;
    public int shrinkPerCycle = 2;

    private Queue<MinoEffect> pool = new Queue<MinoEffect>();
    private List<MinoEffect> allObjects = new List<MinoEffect>();

    void Start()
    {
        InvokeRepeating(nameof(CheckAndShrinkPool), shrinkCheckInterval, shrinkCheckInterval);
    }

    public void Initialize()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddNewObjectToPool();
        }
    }

    private void AddNewObjectToPool()
    {
        GameObject prefab = minoPrefabs[Random.Range(0, minoPrefabs.Length)];
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        MinoEffect effect = obj.GetComponent<MinoEffect>();
        pool.Enqueue(effect);
        allObjects.Add(effect);
    }

    public MinoEffect Get(Vector3 position, Transform parent)
    {
        if (pool.Count == 0)
        {
            AddNewObjectToPool();
        }

        MinoEffect effect = pool.Dequeue();
        effect.transform.position = position;
        effect.transform.SetParent(parent, true);
        effect.gameObject.SetActive(true);
        effect.Init(this);
        return effect;
    }

    public void ReturnToPool(MinoEffect effect)
    {
        effect.transform.SetParent(transform);
        effect.gameObject.SetActive(false);
        pool.Enqueue(effect);
    }

    private void CheckAndShrinkPool()
    {
        int totalCount = allObjects.Count;
        int currentPoolCount = pool.Count;

        bool canShrink = totalCount > initialPoolSize && currentPoolCount > shrinkThreshold;
        if (!canShrink) return;

        int shrinkable = Mathf.Min(shrinkPerCycle, currentPoolCount, totalCount - initialPoolSize);

        for (int i = 0; i < shrinkable; i++)
        {
            MinoEffect effect = pool.Dequeue();
            allObjects.Remove(effect);
            Destroy(effect.gameObject);
        }
    }
}