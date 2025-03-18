using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    void OnEnable()
    {

    }

    private void Update()
    {
        if (gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            GameStartTracker.IsHavetobeReset = true;
            Scene.Controller.LoadScene(Scene.MainScene);
        }
    }
}
