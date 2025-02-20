using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject loadingEffectPrefab;

    private LoadingManager currentLoading;

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
        currentLoading = Instantiate(loadingEffectPrefab).GetComponent<LoadingManager>();

        StartCoroutine(LoadSceneByCoroutine(sceneName));
    }

    private IEnumerator LoadSceneByCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(currentLoading.GetComponent<LoadingManager>().FadeDuration);

        if (Scene.mapManager != null) switch (sceneName)
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

        SceneManager.LoadScene(sceneName);
    }

    public void OnClearScene()
    {
        Scene.gameManager.ClearCurrentStage();
    }
}
