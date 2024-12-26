using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    [SerializeField] private MapScrollView mapScrollView;
    [SerializeField] private List<List<Stage>> mapInfo;
    [SerializeField] private GameObject stagePrefab;


    [SerializeField] private float stageVerticalOffset;
    [SerializeField] private float stageHorizontalInterval;
    [SerializeField] private float stageVerticalInterval;

    public readonly int floorNumber = 15;
    
    private void Awake()
    {
        GenerateMap(out mapInfo);
        AllocateStageGameObject(mapInfo);
        SetStagePosition(mapInfo);
        SetScrollView(mapScrollView, mapInfo);
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
