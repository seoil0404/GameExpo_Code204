using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject loadingEffectPrefab;
    [SerializeField] private GameObject tutorialViewPrefab;

    private LoadingManager currentLoading;

    private bool IsSceneMoving = false;

    public bool IsGameSceneFirstLoading = true;

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
        if (!IsSceneMoving)
        {
            currentLoading = Instantiate(loadingEffectPrefab).GetComponent<LoadingManager>();

            StartCoroutine(LoadSceneByCoroutine(sceneName));
        }
    }

    private IEnumerator LoadSceneByCoroutine(string sceneName)
    {
        IsSceneMoving = true;

        yield return new WaitForSeconds(currentLoading.GetComponent<LoadingManager>().FadeDuration);

        IsSceneMoving = false;

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
                case Scene.MainScene:
                    IsGameSceneFirstLoading = true;
                    Scene.mapManager.IsAllowOpen = true;
                    Scene.mapManager.IsStatic = false;
                    if (Scene.mapManager != null) Destroy(Scene.mapManager.highMap);
                    break;
                default:
                    Scene.mapManager.DisableMap();
                    IsGameSceneFirstLoading = true;
                    Scene.mapManager.IsAllowOpen = true;
                    Scene.mapManager.IsStatic = false;
                    break;
            }

        SceneManager.LoadScene(sceneName);

        if(sceneName == Scene.GameScene && IsGameSceneFirstLoading)
        {
            DontDestroyOnLoad(Instantiate(tutorialViewPrefab));
        }
    }

    public void OnClearScene()
    {
        Scene.gameManager.ClearCurrentStage();
    }
}
