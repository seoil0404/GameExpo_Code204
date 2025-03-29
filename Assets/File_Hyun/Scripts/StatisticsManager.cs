using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance;

    // 각 모험당 통계
    public int HighestFloorReached;
    public int BossesKilledThisRun;
    public int MonstersKilledThisRun;
    public int TotalDamageDealtThisRun;
    public int ClearLineCountThisRun;
    public int BossCoinsEarnedThisRun;

    // 총 게임 통계
    private int totalFloorsClimbed;
    private int totalAdventures;
    private int successfulAdventures;
    private float shortestAdventureTime = float.MaxValue;
    private int totalEnemiesKilled;
    private int totalBossesKilled;
    private int unlockedElements;
    private float totalPlayTime;

    private float adventureStartTime;

    public GoldData goldData;
    public int firstGold;

    public int CurrentRoom = 0;
    public int CurrentFloor = 1;

    private string previousSceneName = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTotalStatistics();
            SceneManager.sceneLoaded += OnSceneLoaded;
            previousSceneName = SceneManager.GetActiveScene().name;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(setFirstGold());
    }

    private void Update()
    {
        Debug.LogWarning($"{CurrentFloor}   /   {CurrentRoom}");
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(setFirstGold());
        string newSceneName = scene.name;
        if (previousSceneName == "CharacterScene" && newSceneName == "MapScene")
            StartAdventure();
        previousSceneName = newSceneName;
    }

    private IEnumerator setFirstGold()
    {
        yield return null;
        firstGold = goldData.InGameGold;
    }

    public void StartAdventure()
    {
        ResetAdventureStats();
        adventureStartTime = Time.time;
        totalAdventures++;
        Debug.Log("모험 시작!");
    }

    public void CompleteAdventure()
    {
        successfulAdventures++;

        float adventureTime = Time.time - adventureStartTime;
        if (adventureTime < shortestAdventureTime)
        {
            shortestAdventureTime = adventureTime;
            Debug.Log($"최단 모험 시간 갱신: {shortestAdventureTime}초");
        }

        UpdateTotalStatistics();
    }

    public void FailAdventure()
    {
        UpdateTotalStatistics();
        ResetAdventureStats();
    }

    private void ResetAdventureStats()
    {
        CurrentRoom = 0;
        CurrentFloor = 1;
        HighestFloorReached = 0;
        BossesKilledThisRun = 0;
        MonstersKilledThisRun = 0;
        TotalDamageDealtThisRun = 0;
        ClearLineCountThisRun = 0;
        BossCoinsEarnedThisRun = 0;
    }

    private void UpdateTotalStatistics()
    {
        totalFloorsClimbed += HighestFloorReached;
        totalEnemiesKilled += MonstersKilledThisRun + BossesKilledThisRun;
        totalBossesKilled += BossesKilledThisRun;
        totalPlayTime += Time.time - adventureStartTime;
        SaveTotalStatistics();
    }

    private void SaveTotalStatistics()
    {
        PlayerPrefs.SetInt("TotalFloorsClimbed", totalFloorsClimbed);
        PlayerPrefs.SetInt("TotalAdventures", totalAdventures);
        PlayerPrefs.SetInt("SuccessfulAdventures", successfulAdventures);
        PlayerPrefs.SetFloat("ShortestAdventureTime", shortestAdventureTime);
        PlayerPrefs.SetInt("TotalEnemiesKilled", totalEnemiesKilled);
        PlayerPrefs.SetInt("TotalBossesKilled", totalBossesKilled);
        PlayerPrefs.SetInt("UnlockedElements", unlockedElements);
        PlayerPrefs.SetFloat("TotalPlayTime", totalPlayTime);
        PlayerPrefs.Save();

        Debug.Log("[통계] 총 통계 저장 완료");
    }

    private void LoadTotalStatistics()
    {
        totalFloorsClimbed = PlayerPrefs.GetInt("TotalFloorsClimbed", 0);
        totalAdventures = PlayerPrefs.GetInt("TotalAdventures", 0);
        successfulAdventures = PlayerPrefs.GetInt("SuccessfulAdventures", 0);
        shortestAdventureTime = PlayerPrefs.GetFloat("ShortestAdventureTime", float.MaxValue);
        totalEnemiesKilled = PlayerPrefs.GetInt("TotalEnemiesKilled", 0);
        totalBossesKilled = PlayerPrefs.GetInt("TotalBossesKilled", 0);
        unlockedElements = PlayerPrefs.GetInt("UnlockedElements", 0);
        totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0f);

        Debug.Log("[통계] 총 통계 불러오기 완료");
    }
}