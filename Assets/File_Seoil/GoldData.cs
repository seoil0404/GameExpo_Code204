using UnityEngine;

[CreateAssetMenu(fileName = "GoldData", menuName = "Scriptable Objects/GoldData")]
public class GoldData : ScriptableObject
{
    [SerializeField] private int inGameGold;
    [SerializeField] private int bossGold;

    public GoldShowManager ShowManager;

    public int InGameGold
    {
        get => inGameGold;
        set
        {
            inGameGold = value;
            ShowManager.UpdateGoldData();
        }
    }

    public int BossGold
    {
        get => bossGold;
        set
        { 
            bossGold = value;
        }
    }
}
