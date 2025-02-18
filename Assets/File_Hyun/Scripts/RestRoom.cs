using UnityEngine;

public class RestRoom : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;
    public float HealAmount = 0.4f;
    public void HealCharacter()
    {
        if (true)
        {
            Debug.Log("Èú!");

            int tamp = characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp;
            Debug.Log($"{tamp}");

            characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp = 100;

            GameObject.Find("SceneController").GetComponent<SceneController>().OnClearScene();
            Debug.Log("ÈÞ½Ä¹æ Á¾·á");
        }
    }
}