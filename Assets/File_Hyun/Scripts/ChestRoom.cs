using UnityEngine;
using UnityEngine.UI;

public class ChestRoom : MonoBehaviour
{
    public void GetRelics()
    {
        if (true)
        {
            Debug.Log("겟 프-리 유물!");

            Scene.Controller.OnClearScene();
            Debug.Log("상자방 종료");
        }
    }
}