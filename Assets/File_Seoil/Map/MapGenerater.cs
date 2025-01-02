using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.SceneManagement;
using UnityEngine;

using Random = UnityEngine.Random;

public class MapGenerater : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject stagePrefab;
    [SerializeField] private GameObject edgePrefab;

    [Header("MonoBahavior")]
    [SerializeField] private GameObject map;
    [SerializeField] private MapScrollView mapScrollView;
    [SerializeField] private Transform mapTransform;

    [Header("Place Offset")]
    [SerializeField] private float stageVerticalOffset;
    [SerializeField] private float stageHorizontalInterval;
    [SerializeField] private float stageVerticalInterval;

    private List<List<Stage>> mapInfo;
    public readonly int floorNumber = 15;
    
    private void Awake()
    {
        GenerateMap(out mapInfo);
        AllocateStageGameObject(mapInfo);
        SetStagePosition(mapInfo);
        SetScrollView(mapScrollView, mapInfo);
        GenerateEdges();
    }

    private void SetScrollView(MapScrollView scrollView, List<List<Stage>> list)
    {
        scrollView.minValue = -(list.Count * stageVerticalInterval) + 200;
        scrollView.maxValue = 0;
    }

    private void SetStagePosition(List<List<Stage>> list)
    {
        int floorIndex = 0;
        foreach(var floor in list)
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

    private void AllocateStageGameObject(in List<List<Stage>> list)
    {
        foreach (var floor in list)
        {
            foreach(var stage in floor)
            {
                stage.AllocatedStageObject = Instantiate(stagePrefab, mapScrollView.transform);
            }
        }
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

    private void GenerateMap(out List<List<Stage>> list)
    {
        list = new();

        for (int floorIndex = 0; floorIndex < floorNumber - 1; floorIndex++)
        {
            list.Add(new());

            List<Stage> currentList = list[^1];

            int roomCount = UnityEngine.Random.Range(2, 4);
            for (int roomIndex = 0; roomIndex < roomCount; roomIndex++)
            {
                currentList.Add(new());
            }
        }

        GenerateBossMap(list);
    }

    // Add boss map at last index of the list
    private void GenerateBossMap(List<List<Stage>> list)
    {
        list.Add(new());

        List<Stage> currentList = list[^1];
        currentList.Add(new());
    }
}
