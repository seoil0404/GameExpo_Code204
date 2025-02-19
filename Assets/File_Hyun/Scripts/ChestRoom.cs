using UnityEngine;
using UnityEngine.UI;

public class ChestRoom : MonoBehaviour
{
    public void GetRelics()
    {
            Debug.Log("유물 얻음");

    }

    public void EndChestRoom()
    {
            GameObject.Find("SceneController").GetComponent<SceneController>().OnClearScene();
            Debug.Log("상자방 종료");
    }
}