using UnityEngine;

public class ClearChopRoom : MonoBehaviour
{
    public void Clear()
    {
        StatisticsManager.Instance.CurrentRoom++;
        Scene.Controller.OnClearScene();
        Debug.Log("������ ����");
    }
}
