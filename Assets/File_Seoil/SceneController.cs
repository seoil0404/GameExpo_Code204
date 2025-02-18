using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Awake()
    {
        if (Scene.Controller != null) Destroy(gameObject);
        else
        {
            Scene.Controller = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(Scene.SceneType sceneType)
    {
        LoadScene(Scene.GetSceneNameByType(sceneType));
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnClearScene()
    {

    }
}
