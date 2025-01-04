using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using Random = UnityEngine.Random;

public class MapGenerater : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject stagePrefab;
    [SerializeField] private GameObject edgePrefab;

    [Header("MonoBahavior")]
    [SerializeField] private GameObject map;
    [SerializeField] private MapScrollView mapScrollView;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Transform mapTransform;
    
    [Header("Place Offset")]
    [SerializeField] private float stageVerticalOffset;
    [SerializeField] private float stageHorizontalInterval;
    [SerializeField] private float stageVerticalInterval;

    [Header("Stage Type Probability")]
    [SerializeField] private float specialCombatProbablity;
    [SerializeField] private float chestProbability;
    [SerializeField] private float restProbability;
    [SerializeField] private float eventProbability;
    [SerializeField] private float combatProbability;

    [Header("Stage Sprite")]
    [SerializeField] private Sprite specialCombatSprite;
    [SerializeField] private Sprite chestSprite;
    [SerializeField] private Sprite restSprite;
    [SerializeField] private Sprite eventSprite;
    [SerializeField] private Sprite combatSprite;
    [SerializeField] private Sprite bossSprite;
    [SerializeField] private Sprite clearSprite;
    [SerializeField] private Sprite fightingSprite;

    private List<List<Stage>> mapInfo;
    private StageCountInfo stageCountInfo;
    public readonly int floorNumber = 15;

    public IReadOnlyList<IReadOnlyList<Stage>> MapInfo => mapInfo;
    public Sprite ClearSprite => clearSprite;
    public Sprite FightingSprite => fightingSprite;
    
    private void Awake()
    {
        GenerateMap();
        AllocateStageGameObject();
        SetStagePosition();
        SetScrollView();
        GenerateEdges();
    }
    private void SetScrollView()
    {
        mapScrollView.minValue = -(mapInfo.Count * stageVerticalInterval) + 200;
        mapScrollView.maxValue = 0;
    }
    private void SetStagePosition()
    {
        int floorIndex = 0;

        foreach(var floor in mapInfo)
        {
            if(floor.Count == 1)
            {
                floor[^1].rectTransform.anchoredPosition = new Vector3(
                    0,
                    stageVerticalOffset + stageVerticalInterval * floorIndex,
                    floor[^1].rectTransform.position.z
                );
                continue;
            }

            int stageIndex = 0;
            
            float stageHorizontalOffset = 0;

            if (floor.Count % 2 == 0)
                stageHorizontalOffset = stageHorizontalInterval / 2;

            foreach(var stage in floor)
            {
                stage.rectTransform.anchoredPosition = new Vector3(
                    stageHorizontalOffset + stageHorizontalInterval * (stageIndex - 1),
                    stageVerticalOffset + stageVerticalInterval * floorIndex,
                    stage.rectTransform.position.z
                );

                stageIndex++;
            }
            
            floorIndex++;
        }
    }
    private void AllocateStageGameObject()
    {
        foreach (var floor in mapInfo)
        {
            foreach(var stage in floor)
            {
                stage.AllocatedStageObject = Instantiate(stagePrefab, mapScrollView.transform);
                
                stage.AllocatedStageObject.GetComponent<StageMonoBehavior>().mapManager = mapManager;
                
                SetStageType(stage, GetRandomStageType());
            }
        }

        foreach(var stage in mapInfo[0]) SetStageType(stage, Stage.StageType.Combat);

        SetStageType(mapInfo[^1][^1], Stage.StageType.Boss);

        while(stageCountInfo.GetZeroStageCount() != Stage.StageType.None)
        {
            AddMissingTypeStage(stageCountInfo.GetZeroStageCount());
        }
    }
    private void AddMissingTypeStage(Stage.StageType type)
    {
        int floorRand = Random.Range(1, mapInfo.Count - 1);
        int indexRand = Random.Range(0, mapInfo[floorRand].Count);
        
        SetStageType(mapInfo[floorRand][indexRand], type);

        Debug.Log("Added Missing Type Stage");
    }
    private void GenerateEdges()
    {
        for(int index= 0; index < mapInfo.Count - 2; index++)
        {
            Vector2Int[] data = null;

            if (mapInfo[index].Count == 2)
            {
                if (mapInfo[index + 1].Count == 2)
                {
                    data = MapData.array2to2[Random.Range(0, MapData.array2to2.Length)];
                }
                else if (mapInfo[index + 1].Count == 3)
                {
                    data = MapData.array2to3[Random.Range(0, MapData.array2to3.Length)];
                }
            }
            else if (mapInfo[index].Count == 3)
            {
                if (mapInfo[index + 1].Count == 2)
                {
                    data = MapData.array3to2[Random.Range(0, MapData.array3to2.Length)];
                }
                else if (mapInfo[index + 1].Count == 3)
                {
                    data = MapData.array3to3[Random.Range(0, MapData.array3to3.Length)];
                }
            }

            foreach(Vector2Int item in data)
            {
                GenerateEdge(mapInfo[index][item.x], mapInfo[index + 1][item.y]);
            }
        }

        foreach(Stage item in mapInfo[^2])
        {
            GenerateEdge(item, mapInfo[^1][^1]);
        }
    }
    private void GenerateEdge(Stage start, Stage end)
    {
        LineRenderer line = Instantiate(edgePrefab, mapScrollView.transform).GetComponent<LineRenderer>();
        
        start.connect.Add(end);
        
        line.SetPosition(0, start.rectTransform.anchoredPosition3D);
        line.SetPosition(1, end.rectTransform.anchoredPosition3D);
    }
    private void GenerateMap()
    {
        mapInfo = new();

        for (int floorIndex = 0; floorIndex < floorNumber - 1; floorIndex++)
        {
            mapInfo.Add(new());

            List<Stage> currentList = mapInfo[^1];

            int roomCount = Random.Range(2, 4);
            for (int roomIndex = 0; roomIndex < roomCount; roomIndex++)
            {
                currentList.Add(new());
            }
        }

        GenerateBossMap();
    }
    private Stage.StageType GetRandomStageType()
    {
        float rand = Random.Range(
            0, 
            specialCombatProbablity + chestProbability + restProbability + eventProbability + combatProbability
        );

        if(rand < specialCombatProbablity)
        {
            return Stage.StageType.SpecialCombat;
        }

        rand -= specialCombatProbablity;
        
        if(rand < chestProbability)
        {
            return Stage.StageType.Chest;
        }
        
        rand -= chestProbability;
        
        if(rand < restProbability)
        {
            return Stage.StageType.Rest;
        }
        
        rand -= restProbability;
        
        if(rand < eventProbability)
        {
            return Stage.StageType.Event;
        }

        return Stage.StageType.Combat;
    }
    // Add boss map at last index of the list
    private void GenerateBossMap()
    {
        mapInfo.Add(new());

        List<Stage> currentList = mapInfo[^1];
        
        currentList.Add(new());
    }
    private void SetStageType(Stage stage, Stage.StageType type)
    {
        switch (stage.type)
        {
            case Stage.StageType.SpecialCombat:
                stageCountInfo.specialCombatCount--;
                break;
            case Stage.StageType.Chest:
                stageCountInfo.chestCount--;
                break;
            case Stage.StageType.Event:
                stageCountInfo.eventCount--;
                break;
            case Stage.StageType.Rest:
                stageCountInfo.restCount--;
                break;
            case Stage.StageType.Combat:
                stageCountInfo.combatCount--;
                break;
            case Stage.StageType.Boss:
                stageCountInfo.bossCount--;
                break;
            default:
                break;
        }

        stage.type = type;
        
        switch (type)
        {
            case Stage.StageType.SpecialCombat:
                stage.objectSpriteRenderer.sprite = specialCombatSprite;
                stageCountInfo.specialCombatCount++;
                break;
            case Stage.StageType.Chest:
                stage.objectSpriteRenderer.sprite = chestSprite;
                stageCountInfo.chestCount++;
                break;
            case Stage.StageType.Event:
                stage.objectSpriteRenderer.sprite = eventSprite;
                stageCountInfo.eventCount++;
                break;
            case Stage.StageType.Rest:
                stage.objectSpriteRenderer.sprite = restSprite;
                stageCountInfo.restCount++;
                break;
            case Stage.StageType.Combat:
                stage.objectSpriteRenderer.sprite = combatSprite;
                stageCountInfo.combatCount++;
                break;
            case Stage.StageType.Boss:
                stage.objectSpriteRenderer.sprite = bossSprite;
                stageCountInfo.bossCount++;
                break;
            default:
                Debug.LogError("Invalid Stage Type Error");
                stage.objectSpriteRenderer.sprite = null;
                break;
        }

    }
    private struct StageCountInfo
    {
        public int specialCombatCount;
        public int chestCount;
        public int restCount;
        public int eventCount;
        public int combatCount;
        public int bossCount;

        public Stage.StageType GetZeroStageCount()
        {
            if(specialCombatCount == 0) return Stage.StageType.SpecialCombat;
            if(chestCount == 0) return Stage.StageType.Chest;
            if(restCount == 0) return Stage.StageType.Rest;
            if (eventCount == 0) return Stage.StageType.Event;
            if (combatCount == 0) return Stage.StageType.Combat;
            if (bossCount == 0) return Stage.StageType.Boss;
            return Stage.StageType.None;
        }
    }
}