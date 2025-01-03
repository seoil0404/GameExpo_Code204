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

    public StageType Type { get; private set; }


    public enum StageType
    {
        SpecialCombat, Combat, Chest, Rest, Event, Boss
    }

    public Stage(StageType type)
    {
        connect = new List<Stage>();
        Type = type;
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
    }
}
