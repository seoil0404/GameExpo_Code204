using UnityEngine;
using UnityEngine.UI;

public class RestRoom : MonoBehaviour
{
    public float HealAmount = 0.4f;
    public void HealCharacter()
    {
        if (true)
        {
            Debug.Log("��!");

            GameObject.Find("SceneController").GetComponent<SceneController>().OnClearScene();
            Debug.Log("�޽Ĺ� ����");
        }
    }
}