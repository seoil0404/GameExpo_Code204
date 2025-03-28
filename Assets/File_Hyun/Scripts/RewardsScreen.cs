using UnityEngine;
using UnityEngine.UI;

public class RewardsScreen : MonoBehaviour
{
    public GoldData goldData;
    public Text GoldEarned;

    void OnEnable()
    {
        GoldEarned.text = "+" + (goldData.InGameGold - StatisticsManager.Instance.firstGold) + "G";
    }

    public void CloseRewardsScreen()
    {
        Scene.Controller.OnClearScene();
    }
}
