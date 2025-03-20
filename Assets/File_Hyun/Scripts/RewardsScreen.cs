using UnityEngine;
using UnityEngine.UI;

public class RewardsScreen : MonoBehaviour
{
    public GoldData goldData;
    public Text GoldEarned;

    private int firstGold;

    private bool isFirstInit = true;

    private void OnDisable()
    {
        if (isFirstInit)
        {
            firstGold = goldData.InGameGold;
            isFirstInit = false;
        }
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
