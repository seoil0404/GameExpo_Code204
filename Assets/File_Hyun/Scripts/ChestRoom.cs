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
        Scene.Controller.OnClearScene();
        Debug.Log("���ڹ� ����");
    }
}