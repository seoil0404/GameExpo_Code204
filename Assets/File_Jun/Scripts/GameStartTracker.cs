using UnityEngine;

public class GameStartTracker : MonoBehaviour
{
    public static GameStartTracker instance;

    public static bool IsHavetobeReset { get; set; } = true;

    public static bool IsUsedMoneyBag { get; set; } = false;

    public static bool IsUsedTotemOfResistance { get; set; } = false;

    public static bool IsUsedGoldenApple { get; set; } = false;

    public static bool IsUsedRingofTime { get; set; } = false;




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
