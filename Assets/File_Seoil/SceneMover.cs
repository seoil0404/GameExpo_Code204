using UnityEngine;

public class SceneMover : MonoBehaviour
{
    public Scene.SceneType MoveSceneType;

    public void MoveScene()
    {
        Scene.Controller.LoadScene(MoveSceneType);
    }
}
