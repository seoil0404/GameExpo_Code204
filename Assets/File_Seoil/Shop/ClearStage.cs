using UnityEngine;

public class ClearStage : MonoBehaviour
{
    public void Clear()
    {
        Scene.Controller.OnClearScene();
    }
}
