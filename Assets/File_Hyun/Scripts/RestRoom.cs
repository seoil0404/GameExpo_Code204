using UnityEngine;
using UnityEngine.UI;

public class RestRoom : MonoBehaviour
{
    public float HealAmount = 0.4f;
    public void HealCharacter()
    {
        if (true)
        {
            Debug.Log("Èú!");

            GameObject.Find("SceneController").GetComponent<SceneController>().OnClearScene();
            Debug.Log("ÈÞ½Ä¹æ Á¾·á");
        }
    }
}