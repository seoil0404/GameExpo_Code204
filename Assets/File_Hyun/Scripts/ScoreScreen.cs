using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    public Text CurrentFloor;
    public Text HighestFloor;
    public Text BossesKilledThisRun;
    public Text MonstersKilledThisRun;
    public Text TotalDamageDealtThisRun;
    public Text ClearLineCountThisRun;
    public Text BossCoinsEarnedThisRun;

    void OnEnable()
    {
        CurrentFloor.text = $"[ {StatisticsManager.Instance.CurrentFloor}   /   {StatisticsManager.Instance.CurrentRoom + 1} ]";
        HighestFloor.text = $"{StatisticsManager.Instance.HighestFloorReached}";
        BossesKilledThisRun.text = $"{StatisticsManager.Instance.CurrentFloor - 1}����";
        MonstersKilledThisRun.text = $"{StatisticsManager.Instance.MonstersKilledThisRun - (StatisticsManager.Instance.CurrentFloor - 1)}����";
        TotalDamageDealtThisRun.text = $"{StatisticsManager.Instance.TotalDamageDealtThisRun}";
        ClearLineCountThisRun.text = $"{StatisticsManager.Instance.ClearLineCountThisRun}��";
        BossCoinsEarnedThisRun.text = "-";
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
