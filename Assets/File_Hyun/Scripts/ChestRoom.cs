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
            // 상자에서 얻을 수 없는 유물 제외
            if (treasure == TreasureType.TalismanOfPower)
                continue;

            // 이미 보유한 유물 제외
            if (Array.Exists(combatData.TreasureData, t => t == treasure))
                continue;

            // 전용 유물이면 현재 캐릭터와 일치하는 경우만 허용
            if (exclusiveRelics.ContainsValue(treasure) && treasure != exclusiveRelicForCurrentCharacter)
                continue;

            candidateTreasures.Add(treasure);
        }

        if (candidateTreasures.Count == 0)
        {
            relicNameText.text = "더 이상 얻을 수 있는 유물이 없습니다.";
            return;
        }

        TreasureType selectedTreasure = candidateTreasures[UnityEngine.Random.Range(0, candidateTreasures.Count)];
        combatData.AddTreasureData(selectedTreasure);
        relicNameText.text = $"상자 속에서 {selectedTreasure}을 발견했다.";
    }

    public void EndChestRoom()
    {
        Scene.Controller.OnClearScene();
        StatisticsManager.Instance.CurrentRoom++;
        StatisticsManager.Instance.HighestFloorReached++;
        Debug.Log("상자방 종료");
    }
}