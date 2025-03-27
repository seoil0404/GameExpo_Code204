using UnityEngine;
using UnityEngine.UI;
using System;
using static CombatData;

public class ChestRoom : MonoBehaviour
{
    public CombatData combatData;

    [SerializeField] private SpriteRenderer chestRenderer;
    [SerializeField] private Sprite closedChestSprite;
    [SerializeField] private Sprite openChestSprite;
    [SerializeField] private Text relicNameText;
    [SerializeField] private GameObject Button;

    private bool isChestOpened = false;

    private void Start()
    {
        relicNameText.text = "당신은 모험중 낡고 녹슨 상자를 발견했다.";
        chestRenderer.sprite = closedChestSprite;
    }

    private void OnMouseDown()
    {
        if (!isChestOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isChestOpened = true;
        chestRenderer.sprite = openChestSprite;
        GetRelic();
        Button.SetActive(true);
        Debug.Log("상자를 열었습니다.");
    }

    private void GetRelic()
    {
        TreasureType[] availableTreasures = (TreasureType[])Enum.GetValues(typeof(TreasureType));
        if (availableTreasures.Length == 0) return;

        TreasureType randomTreasure = availableTreasures[UnityEngine.Random.Range(0, availableTreasures.Length)];
        combatData.AddTreasureData(randomTreasure);
        relicNameText.text = $"상자 속에서 {randomTreasure}을 발견했다.";
    }

    public void EndChestRoom()
    {
        Scene.Controller.OnClearScene();
        StatisticsManager.Instance.CurrentRoom++;
        Debug.Log("상자방 종료");
    }
}