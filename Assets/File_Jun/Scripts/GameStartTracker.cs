using UnityEngine;

public class GameStartTracker : MonoBehaviour
{
    public static GameStartTracker instance;

    public static bool IsHavetobeReset { get; set; } = true;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log($"[GameStartTracker] Awake ½ÇÇàµÊ, IsHavetobeReset: {IsHavetobeReset}");
    }
}