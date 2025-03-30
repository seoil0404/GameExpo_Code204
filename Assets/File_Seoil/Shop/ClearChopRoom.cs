using UnityEngine;

public class ClearChopRoom : MonoBehaviour
{
    public void Clear()
    {
        StatisticsManager.Instance.CurrentRoom++;
        StatisticsManager.Instance.HighestFloorReached++;
        Debug.Log("상점방 종료");
        Scene.Controller.OnClearScene();
    }
}