using UnityEngine;
using UnityEngine.UI;

public class ChestRoom : MonoBehaviour
{
    public void GetRelics()
    {
            Debug.Log("���� ����");

    }

    public void EndChestRoom()
    {
            GameObject.Find("SceneController").GetComponent<SceneController>().OnClearScene();
            Debug.Log("���ڹ� ����");
    }
}