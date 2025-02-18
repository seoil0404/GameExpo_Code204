using UnityEngine;

public class SceneControllerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject sceneController;

    private void Awake()
    {
        if(Scene.Controller == null) Instantiate(sceneController);
    }
}
