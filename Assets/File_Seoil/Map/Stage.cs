using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage
{
    public List<Stage> connect;
    public Image objectSpriteRenderer;

    private GameObject allocatedStageObject;
    private RectTransform stageObjectRectTransform;

    public StageType stageType;

    public Sprite AllocatedSprite;

    [Serializable]
    public enum StageType
    {
        None, SpecialCombat, Combat, Chest, Rest, Event, Boss, MagicStore
    }

    public Stage()
    {
        connect = new List<Stage>();
        stageType = StageType.None;
    }

    public RectTransform rectTransform
    {
        get
        {
            if (stageObjectRectTransform != null)
            {
                return stageObjectRectTransform;
            }
            else
            {
                Debug.LogError("The RectTransform of Stage is not Allocated");
                return null;
            }
        }
    }

    public GameObject AllocatedStageObject
    {
        set
        {
            allocatedStageObject = value;

            stageObjectRectTransform = allocatedStageObject.GetComponent<RectTransform>();
            objectSpriteRenderer = allocatedStageObject.GetComponent<Image>();
        }
        get => allocatedStageObject;
    }
}
