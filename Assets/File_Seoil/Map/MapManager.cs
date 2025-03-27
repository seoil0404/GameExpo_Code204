using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private bool isStatic;

    [Header("Scriptable")]
    [SerializeField] private CombatData combatData;

    [Header("MonoBehavior")]
    public GameObject highMap;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TreasureShowManager treasureShowManager;
    [SerializeField] private GameObject map;
    [SerializeField] private KeyManager keyManager;
    [SerializeField] private MapGenerater mapGenerater;
    [SerializeField] private GoldShowManager goldShowManager;

    private List<Stage> currentList;
    private Stage currentStage;
    private bool isCurrentStageCleared = true;

    private bool isMapEnable = false;
    
    [SerializeField] private bool isAllowOpen = true;

    public bool IsStatic
    {
        set 
        { 
            isStatic = value;
            if (isStatic) EnableMap();
        }
    }

    public bool IsAllowOpen
    {
        set
        {
            isAllowOpen = value;
            if (!isAllowOpen) DisableMap();
        }
    }
    
    public bool IsMapEnable
    {
        get
        {
            return isMapEnable;
        }
    }

    public void ClearCurrentStage()
    {
        if (currentStage == null)
        {
            Debug.LogError("Nothing To Clear");
        }
        else
        {
            if(currentStage.stageType == Stage.StageType.Combat && Scene.Controller.IsGameSceneFirstLoading) 
                Scene.Controller.IsGameSceneFirstLoading = false;

            isCurrentStageCleared = true;

            currentStage.objectSpriteRenderer.sprite = currentStage.AllocatedSprite;

            currentStage.AllocatedStageObject.GetComponent<StageMonoBehavior>().OnCleared();

            if (currentStage.stageType == Stage.StageType.Boss)
            {
                ClearCurrentLevel();
                return;
            }

            Scene.Controller.LoadScene(Scene.MapScene);
        }
    }

    public void ClearGame()
    {
        Debug.Log("Game Cleared");
    }

    private void Awake()
    {
        if (Scene.mapManager == null)
        {
            DontDestroyOnLoad(highMap);

            Scene.mapManager = this;
            
            gameManager.Initialize();
            mapGenerater.Initialize();
            treasureShowManager.Initialize();
            goldShowManager.Initialize();
            Initialize();
        }
        else Destroy(highMap);

        if (isStatic) EnableMap();
    }

    private void Initialize()
    {
        currentList = mapGenerater.MapInfo[0];

        foreach(Stage item in currentList)
        {
            item.AllocatedStageObject.GetComponent<StageButtonAnimation>().IsAllowClick = true;
        }
    }

    private void Update()
    {
        HandleMapState();
    }

    private void ClearCurrentLevel()
    {
        switch(combatData.HabitatType)
        {
            case EnemyData.HabitatType.Forest:
                combatData.HabitatType = EnemyData.HabitatType.Castle;
                break;
            case EnemyData.HabitatType.Castle:
                combatData.HabitatType = EnemyData.HabitatType.DevilCastle;
                break;
            case EnemyData.HabitatType.DevilCastle:
                ClearGame();
                break;
        }

        Debug.Log("[MapManager] Clear Current Level");
        StatisticsManager.Instance.CurrentFloor++;
        StatisticsManager.Instance.CurrentRoom = 0;
        mapGenerater.GenerateNextLevelMap();
    }

    private void HandleMapState()
    {
        if (Input.GetKeyDown(keyManager.MapKey) && !isStatic && isAllowOpen)
        {
            if (map.activeSelf)
            {
                DisableMap();
            }
            else
            {
                EnableMap();
            }
        }
    }
    public void EnableMap()
    {
        goldShowManager.HideGoldData();
        treasureShowManager.Hide();
        
        map.SetActive(true);
    }
    public void DisableMap()
    {
        goldShowManager.ShowGoldData();
        treasureShowManager.Show();

        map.SetActive(false);
    }
    public void ClickedStage(GameObject stageObject)
    {
        if (isCurrentStageCleared)
        {
            foreach (Stage stage in currentList)
            {
                if (stage.AllocatedStageObject == stageObject)
                {
                    currentStage = stage;
                    isCurrentStageCleared = false;

                    foreach(Stage item in currentList)
                    {
                        item.AllocatedStageObject.GetComponent<StageButtonAnimation>().IsAllowClick = false;
                    }

                    currentList = stage.connect;

                    foreach (Stage item in currentList)
                    {
                        item.AllocatedStageObject.GetComponent<StageButtonAnimation>().IsAllowClick = true;
                    }

                    currentStage.objectSpriteRenderer.sprite = mapGenerater.FightingSprite;

                    gameManager.OnStageStarted(stage.stageType);
                }
            }
        }
    }
}