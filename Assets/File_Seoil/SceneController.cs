using UnityEngine;
using UnityEngine.SceneManagement;
using static Scene;

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
        
        switch (sceneType)
        {
            case Scene.SceneType.MapScene:
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = true; 
                break;
            case Scene.SceneType.ChestRoom:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = false;
                break;
            case Scene.SceneType.GameScene:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = false;
                break;
            case Scene.SceneType.EventRoom:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = false;
                break;
            case Scene.SceneType.RestRoom:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = false;
                break;
            case Scene.SceneType.ShopRoom:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = false;
                break;
            default:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = false;
                Scene.mapManager.IsStatic = false;
                break;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        switch (sceneName)
        {
            case Scene.MapScene:
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = true;
                break;
            case Scene.ChestRoom:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = false;
                break;
            case Scene.GameScene:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = false;
                break;
            case Scene.EventRoom:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = false;
                break;
            case Scene.RestRoom:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = false;
                break;
            case Scene.ShopRoom:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = true;
                Scene.mapManager.IsStatic = false;
                break;
            default:
                Scene.mapManager.DisableMap();
                Scene.mapManager.IsAllowOpen = false;
                Scene.mapManager.IsStatic = false;
                break;
        }
    }

    public void OnClearScene()
    {

    }
}
