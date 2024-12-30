using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public List<Stage> connect;
    private GameObject allocatedStageObject;
    private RectTransform stageObjectRectTransform;

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
        }
    }
}
