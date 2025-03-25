using UnityEngine;
using UnityEngine.SceneManagement;

public class MinoClickEffectManager : MonoBehaviour
{
    public static MinoClickEffectManager Instance { get; private set; }

    [Header("미노 이펙트")]
    public GameObject[] minoPrefabs;
    public Color[] possibleColors;
    public int minCount = 4;
    public int maxCount = 6;
    public float scaleMultiplier = 0.1f;

    private MinoEffectPool pool;
    private bool isEffectEnabled = true;

    public bool IsEffectEnabled // MinoClickEffectManager.Instance.IsEffectEnabled

    {
        get => isEffectEnabled;
        set => isEffectEnabled = value;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += (scene, mode) => isEffectEnabled = true;
    }

    void Start()
    {
        GameObject poolObj = new("MinoEffectPool");
        poolObj.transform.SetParent(this.transform);
        pool = poolObj.AddComponent<MinoEffectPool>();
        pool.minoPrefabs = minoPrefabs;
        pool.Initialize();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            int count = Random.Range(minCount, maxCount + 1);
            for (int i = 0; i < count; i++)
            {
                SpawnMino(mouseWorldPos);
            }
        }
    }

    void SpawnMino(Vector3 position)
    {
        MinoEffect mino = pool.Get(position, this.transform);
        mino.transform.localScale = Vector3.one * scaleMultiplier;

        if (mino.TryGetComponent(out SpriteRenderer sr))
        {
            sr.color = possibleColors[Random.Range(0, possibleColors.Length)];

            //항상 최상단 유지
            sr.sortingLayerName = "AlwaysOnTop";
            sr.sortingOrder = 10;
        }

        float angle = Random.Range(-40f, 40f);
        Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
        float power = Random.Range(3f, 5f);
        mino.Launch(dir, power);
    }
}