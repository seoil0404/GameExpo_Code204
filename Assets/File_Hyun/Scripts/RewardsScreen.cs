using UnityEngine;
using UnityEngine.UI;

public class RewardsScreen : MonoBehaviour
{
    public GoldData goldData;
    public Text GoldEarned;

    private int firstGold;

    private void Start()
    {
        firstGold = goldData.InGameGold;
    }

    void OnEnable()
    {
        GoldEarned.text = "+" + (goldData.InGameGold - firstGold) + "G";
    }

    public void CloseRewardsScreen()
    {
        Scene.Controller.OnClearScene();
    }
}
