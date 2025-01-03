using System.Dynamic;
using UnityEngine;

public class StageMonoBehavior : MonoBehaviour
{
    public MapManager mapManager { set; private get; }

    public void Clicked(GameObject stageObject)
    {
        mapManager.ClickedStage(stageObject);
    }
}
