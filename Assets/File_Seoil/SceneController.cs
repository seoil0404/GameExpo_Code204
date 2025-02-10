using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadCharacterScene()
    {
        SceneManager.LoadScene("CharacterScene");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnClearScene()
    {

    }
}
