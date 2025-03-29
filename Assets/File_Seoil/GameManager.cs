using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private CombatData combatData;

    [Header("MonoBehavior")]
    [SerializeField] private MapManager mapManager;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip combatEnterSound;
    [SerializeField] private AudioClip shopEnterSound;

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

    public void OnStageStarted(Stage.StageType stageType)
    {
        switch(stageType)
        {
            case Stage.StageType.Combat:
                combatData.EnemyType = EnemyData.EnemyType.Common;
                Scene.Controller.audioSource.clip = combatEnterSound;
                Scene.Controller.audioSource.Play();
                Scene.Controller.LoadScene(Scene.SceneType.GameScene);
                break;
            case Stage.StageType.SpecialCombat:
                combatData.EnemyType = EnemyData.EnemyType.SpecialCombat;
                Scene.Controller.audioSource.clip = combatEnterSound;
                Scene.Controller.audioSource.Play();
                Scene.Controller.LoadScene(Scene.SceneType.GameScene);
                break;
            case Stage.StageType.Boss:
                combatData.EnemyType = EnemyData.EnemyType.Boss;
                Scene.Controller.audioSource.clip = combatEnterSound;
                Scene.Controller.audioSource.Play();
                Scene.Controller.LoadScene(Scene.SceneType.GameScene);
                break;
            case Stage.StageType.Chest:
                Scene.Controller.LoadScene(Scene.SceneType.ChestRoom);
                break;
            case Stage.StageType.Event:
                Scene.Controller.LoadScene(Scene.SceneType.EventRoom);
                break;
            case Stage.StageType.MagicStore:
                Scene.Controller.audioSource.clip = shopEnterSound;
                Scene.Controller.audioSource.Play();
                Scene.Controller.LoadScene(Scene.SceneType.ShopRoom);
                break;
            case Stage.StageType.Rest:
                Scene.Controller.LoadScene(Scene.SceneType.RestRoom);
                break;
        }
    }
}
