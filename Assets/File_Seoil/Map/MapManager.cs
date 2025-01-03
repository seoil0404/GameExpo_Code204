using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("MonoBehavior")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject map;
    [SerializeField] private KeyManager keyManager;
    [SerializeField] private MapGenerater mapGenerater;

    private IReadOnlyList<Stage> currentList;
    private Stage currentStage;
    private bool isCurrentStageCleared = true;

    private bool isMapEnable = false;
    public bool IsMapEnable
    {
        get
        {
            return isMapEnable;
        }
    }
    public void ClearCurrentStage()
    {
        if (currentStage == null) Debug.LogError("Nothing To Clear");
        else
        {
            isCurrentStageCleared = true;
            currentStage.objectSpriteRenderer.sprite = mapGenerater.ClearSprite;
        }
    }
    private void Update()
    {
        HandleMapState();
    }
    private void HandleMapState()
    {
        if (Input.GetKeyDown(keyManager.MapKey))
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
        map.SetActive(true);
    }
    public void DisableMap()
    {
        map.SetActive(false);
    }
    public void ClickedStage(GameObject stageObject)
    {
        Debug.Log(isCurrentStageCleared);
        if (isCurrentStageCleared)
        {
            if(currentList == null) currentList = mapGenerater.MapInfo[0];

            foreach (Stage stage in currentList)
            {
                if (stage.AllocatedStageObject == stageObject)
                {
                    currentStage = stage;
                    isCurrentStageCleared = false;
                    currentList = stage.connect;

                    ClearCurrentStage();
                }
            }
        }
    }
}