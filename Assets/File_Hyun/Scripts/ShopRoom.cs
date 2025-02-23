using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using static CombatData;

public class ShopRoom : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;

    public GoldData goldData;
    public CombatData combatData;
    public Text[] relicTexts;
    public Button[] relicButtons;
    public Text characterName;

    private TreasureType[] shopRelics;
    private int[] relicPrices;
    private int difficultyLevel;

    void Start()
    {
        characterName.text = characters[GameData.SelectedCharacterIndex - 1].characterData.CharacterName;
        difficultyLevel = FindFirstObjectByType<EnemySpawner>().currentDifficulty;
        Debug.Log($"{difficultyLevel}");
        GenerateShopItems();
    }

    void GenerateShopItems()
    {
        int relicCount = relicButtons.Length;
        List<TreasureType> availableTreasures = new List<TreasureType>((TreasureType[])Enum.GetValues(typeof(TreasureType)));
        shopRelics = new TreasureType[relicCount];
        relicPrices = new int[relicCount];

        for (int i = 0; i < relicCount; i++)
        {
            if (availableTreasures.Count == 0) break;
            int index = UnityEngine.Random.Range(0, availableTreasures.Count);
            shopRelics[i] = availableTreasures[index];
            availableTreasures.RemoveAt(index);

            relicPrices[i] = UnityEngine.Random.Range(70, 101) + (difficultyLevel * 5);

            relicTexts[i].text = shopRelics[i].ToString() + "\n" + relicPrices[i] + " G";
            int captureIndex = i;
            relicButtons[i].onClick.AddListener(() => BuyRelic(captureIndex));
        }
    }

    void BuyRelic(int index)
    {
        if (goldData.InGameGold >= relicPrices[index])
        {
            goldData.InGameGold -= relicPrices[index];
            combatData.AddTreasureData(shopRelics[index]);
            Debug.Log($"{shopRelics[index]} ����");
        }
        else
        {
            Debug.Log("��尡 �����մϴ�!");
        }
    }

    public void HealCharacter()
    {
        if (goldData.InGameGold >= 30)
        {
            int healAmount = UnityEngine.Random.Range(5, 16); // 5~15 ���� ȸ��
            characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp += healAmount;
            Debug.Log($"{healAmount}��ŭ ȸ��");
        }
        else
        {
            Debug.Log("��尡 �����մϴ�!");
        }
    }

    public void ShopEnd()
    {
        Scene.Controller.OnClearScene();
        Debug.Log("������ ���� ����");
    }
}
