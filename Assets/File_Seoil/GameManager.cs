using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("MonoBehavior")]
    [SerializeField] private MapManager mapManager;

    private void Awake()
    {
        if(mapManager == null)
        {
            Debug.LogError("mapManager is not assigned");
        }
    }

    // Call this function when the stage is cleared.
    public void ClearCurrentStage()
    {
        mapManager.ClearCurrentStage();
    }

    public void OnStageStarted(Stage.StageType stageType, Stage.LevelType levelType)
    {
        // Write this.
    }
}
