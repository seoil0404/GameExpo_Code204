using UnityEngine;

public class ClearChopRoom : MonoBehaviour
{
    public void Clear()
    {
        StatisticsManager.Instance.CurrentRoom++;
        StatisticsManager.Instance.HighestFloorReached++;
        Debug.Log("������ ����");
        Scene.Controller.OnClearScene();
    }
}