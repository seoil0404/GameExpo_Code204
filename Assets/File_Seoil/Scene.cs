using System;
using UnityEngine;

public static class Scene
{
    public const string CharacterScene = "CharacterScene";
    public const string ChestRoom = "ChestRoom";
    public const string EventRoom = "EventRoom";
    public const string GameScene = "GameScene";
    public const string MainScene = "MainScene";
    public const string RestRoom = "RestRoom";
    public const string ShopRoom = "ShopRoom";
    public const string MapScene = "MapScene";

    private static SceneController controller;

    public static SceneController Controller
    { 
        set
        {
            controller = value;
        }
        get
        { 
            if(controller == null)
            {
                Debug.LogWarning("Warning : SceneController does not exist");
                return null;
            }
            return controller; 
        } 
    }

    [Serializable]
    public enum SceneType
    { None, CharacterScene, ChestRoom, EventRoom, GameScene, MainScene, RestRoom, ShopRoom, MapScene };

    public static string GetSceneNameByType(SceneType type)
    {
        switch (type)
        {
            case SceneType.CharacterScene:
                return CharacterScene;
            case SceneType.ChestRoom:
                return ChestRoom;
            case SceneType.EventRoom:
                return EventRoom;
            case SceneType.GameScene:
                return GameScene;
            case SceneType.MainScene:
                return MainScene;
            case SceneType.RestRoom:
                return RestRoom;
            case SceneType.ShopRoom:
                return ShopRoom;
            case SceneType.MapScene:
                return MapScene;
            default:
                Debug.LogError("Unknown SceneType");
                return null;
        }
    }

    public static SceneType GetSceneTypeByName(string name)
    {
        switch(name)
        {
            case CharacterScene:
                return SceneType.CharacterScene;
            case ChestRoom:
                return SceneType.ChestRoom;
            case EventRoom:
                return SceneType.EventRoom;
            case GameScene:
                return SceneType.GameScene;
            case MainScene:
                return SceneType.MainScene;
            case RestRoom:
                return SceneType.RestRoom;
            case ShopRoom:
                return SceneType.ShopRoom;
            case MapScene:
                return SceneType.MapScene;
            default:
                Debug.LogError("Unknown Scene Name");
                return SceneType.None;
        }
    }
}
