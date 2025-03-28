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
        relicNameText.text = "����� ������ ���� �콼 ���ڸ� �߰��ߴ�.";
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
        Debug.Log("���ڸ� �������ϴ�.");
    }

    private void GetRelic()
    {
        TreasureType[] availableTreasures = (TreasureType[])Enum.GetValues(typeof(TreasureType));
        if (availableTreasures.Length == 0) return;

        TreasureType randomTreasure = availableTreasures[UnityEngine.Random.Range(0, availableTreasures.Length)];
        combatData.AddTreasureData(randomTreasure);
        relicNameText.text = $"���� �ӿ��� {randomTreasure}�� �߰��ߴ�.";
    }

    public void EndChestRoom()
    {
        Scene.Controller.OnClearScene();
        StatisticsManager.Instance.CurrentRoom++;
        Debug.Log("���ڹ� ����");
    }
}