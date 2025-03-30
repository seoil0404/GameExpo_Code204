using UnityEngine;
using UnityEngine.UI;
using System;
using static CombatData;
using System.Collections.Generic;

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
        Dictionary<int, TreasureType> exclusiveRelics = new Dictionary<int, TreasureType>
    {
        { 1, TreasureType.UniversalGravitation },
        { 2, TreasureType.BusinessAcumen },
        { 3, TreasureType.Condemnation }
    };

        int selectedCharacterIndex = GameData.SelectedCharacterIndex;
        TreasureType exclusiveRelicForCurrentCharacter = exclusiveRelics[selectedCharacterIndex];

        TreasureType[] allTreasures = (TreasureType[])Enum.GetValues(typeof(TreasureType));
        List<TreasureType> candidateTreasures = new List<TreasureType>();

        foreach (TreasureType treasure in allTreasures)
        {
            // ���ڿ��� ���� �� ���� ���� ����
            if (treasure == TreasureType.TalismanOfPower)
                continue;

            // �̹� ������ ���� ����
            if (Array.Exists(combatData.TreasureData, t => t == treasure))
                continue;

            // ���� �����̸� ���� ĳ���Ϳ� ��ġ�ϴ� ��츸 ���
            if (exclusiveRelics.ContainsValue(treasure) && treasure != exclusiveRelicForCurrentCharacter)
                continue;

            candidateTreasures.Add(treasure);
        }

        if (candidateTreasures.Count == 0)
        {
            relicNameText.text = "�� �̻� ���� �� �ִ� ������ �����ϴ�.";
            return;
        }

        TreasureType selectedTreasure = candidateTreasures[UnityEngine.Random.Range(0, candidateTreasures.Count)];
        combatData.AddTreasureData(selectedTreasure);
        relicNameText.text = $"���� �ӿ��� {selectedTreasure}�� �߰��ߴ�.";
    }

    public void EndChestRoom()
    {
        Scene.Controller.OnClearScene();
        StatisticsManager.Instance.CurrentRoom++;
        StatisticsManager.Instance.HighestFloorReached++;
        Debug.Log("���ڹ� ����");
    }
}