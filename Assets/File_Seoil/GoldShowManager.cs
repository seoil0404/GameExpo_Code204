using TMPro;
using UnityEngine;

public class GoldShowManager : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private GoldData goldData;

    [Header("MonoBehavior")]
    [SerializeField] private TextMeshProUGUI inGameGoldText;

    private void Awake()
    {
        goldData.ShowManager = this;

        UpdateGoldData();
    }

    public void UpdateGoldData()
    {
        if(inGameGoldText != null) inGameGoldText.text = goldData.InGameGold.ToString() + "G";
    }
}
