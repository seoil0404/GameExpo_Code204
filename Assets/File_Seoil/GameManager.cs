using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private CombatData combatData;

    [Header("MonoBehavior")]
    [SerializeField] private MapManager mapManager;

    public void Initialize()
    {
        Scene.gameManager = this;

        if (mapManager == null)
        {
            Debug.LogError("mapManager is not assigned");
        }
    }

    public void ClearCurrentStage()
    {
        mapManager.ClearCurrentStage();
    }

    public void OnStageStarted(Stage.StageType stageType, Stage.LevelType levelType)
    {
        switch(levelType)
        {
            case Stage.LevelType.Forest:
                combatData.HabitatType = EnemyData.HabitatType.Forest;
                break;
            case Stage.LevelType.Castle:
                combatData.HabitatType = EnemyData.HabitatType.Castle;
                break;
            case Stage.LevelType.DevilCastle:
                combatData.HabitatType = EnemyData.HabitatType.DevilCastle;
                break;
        }

        switch(stageType)
        {
            case Stage.StageType.Combat:
                combatData.EnemyType = EnemyData.EnemyType.Common;
                Scene.Controller.LoadScene(Scene.SceneType.GameScene);
                break;
            case Stage.StageType.SpecialCombat:
                combatData.EnemyType = EnemyData.EnemyType.SpecialCombat;
                Scene.Controller.LoadScene(Scene.SceneType.GameScene);
                break;
            case Stage.StageType.Boss:
                combatData.EnemyType = EnemyData.EnemyType.Boss;
                Scene.Controller.LoadScene(Scene.SceneType.GameScene);
                break;
            case Stage.StageType.Chest:
                Scene.Controller.LoadScene(Scene.SceneType.ChestRoom);
                break;
            case Stage.StageType.Event:
                Scene.Controller.LoadScene(Scene.SceneType.EventRoom);
                break;
            case Stage.StageType.MagicStore:
                Scene.Controller.LoadScene(Scene.SceneType.ShopRoom);
                break;
            case Stage.StageType.Rest:
                Scene.Controller.LoadScene(Scene.SceneType.RestRoom);
                break;
        }
    }
}
