using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldShowManager : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private GoldData goldData;

    [Header("MonoBehavior")]
    [SerializeField] private TextMeshProUGUI inGameGoldText;
    [SerializeField] private Image goldtextBackGround;

    public static GoldShowManager Instance;

    public void Initialize()
    {
        HideGoldData();

        goldData.ShowManager = this;

        Instance = this;

        UpdateGoldData();
    }

    public void UpdateGoldData()
    {
        if(inGameGoldText != null) inGameGoldText.text = goldData.InGameGold.ToString() + "G";
    }

    public void HideGoldData()
    {
        inGameGoldText.gameObject.SetActive(false);
        goldtextBackGround.gameObject.SetActive(false);
    }

    public void ShowGoldData()
    {
        inGameGoldText.gameObject.SetActive(true);
        goldtextBackGround.gameObject.SetActive(true);
    }
}
