using UnityEngine;
using UnityEngine.UI;

public class ChestRoom : MonoBehaviour
{
    public void GetRelics()
    {
        if (true)
        {
            Debug.Log("�� ��-�� ����!");

            GameObject.Find("SceneController").GetComponent<SceneController>().OnClearScene();
            Debug.Log("���ڹ� ����");
        }
    }
}