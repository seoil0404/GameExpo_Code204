using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopRoom1 : MonoBehaviour
{
    /*
    public static ShopRoom Instance { get; private set; }

    public Text goldText; // 소지 골드 표시

    [Header("미노 상점 UI")]
    public Button[] minoButtons; // 미노 구매 버튼 (4개)
    public Text[] minoPriceTexts; // 미노 정보 + 가격 텍스트 (4개)

    [Header("스크롤 상점 UI")]
    public Button[] scrollButtons; // 스크롤 구매 버튼 (3개)
    public Text[] scrollPriceTexts; // 스크롤 정보 + 가격 텍스트 (3개)

    [Header("난이도(더미)")]
    public int difficulty_dummy = 1;

    private List<int> shopMinos = new(); // 현재 판매 중인 미노
    private List<int> shopScrolls = new(); // 현재 판매 중인 스크롤

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        RefreshShop();
    }

    private void Update()
    {
        goldText.text = $"{InventoryManager.Gold}G";
    }
    private void GenerateMinoShop()
    {
        shopMinos.Clear();
        List<int> availableMinos = new();

        for (int i = 0; i < 8; i++)
        {
            if (!InventoryManager.specialMinos[i])
                availableMinos.Add(i);
        }

        int availableCount = availableMinos.Count;
        while (shopMinos.Count < 4 && availableMinos.Count > 0)
        {
            int index = Random.Range(0, availableMinos.Count);
            shopMinos.Add(availableMinos[index]);
            availableMinos.RemoveAt(index);
        }

        for (int i = 0; i < minoButtons.Length; i++)
        {
            if (i < shopMinos.Count)
            {
                int minoId = shopMinos[i];
                int price = Random.Range(70, 101) + (difficulty_dummy * 5);
                minoButtons[i].interactable = true;
                minoPriceTexts[i].text = $"미노 {minoId} - {price}G";

                int buttonIndex = i;
                minoButtons[i].onClick.RemoveAllListeners();
                minoButtons[i].onClick.AddListener(() => BuyMino(minoId, price, minoButtons[buttonIndex], minoPriceTexts[buttonIndex]));
            }
            else
            {
                minoButtons[i].interactable = false;
                minoPriceTexts[i].text = "재고 없음";
            }
        }
    }

    private void GenerateScrollShop()
    {
        shopScrolls.Clear();
        List<int> availableScrolls = new();

        for (int i = 0; i < 17; i++)
        {
            availableScrolls.Add(i);
        }

        while (shopScrolls.Count < 3 && availableScrolls.Count > 0)
        {
            int index = Random.Range(0, availableScrolls.Count);
            shopScrolls.Add(availableScrolls[index]);
            availableScrolls.RemoveAt(index);
        }

        for (int i = 0; i < scrollButtons.Length; i++)
        {
            if (i < shopScrolls.Count)
            {
                int scrollId = shopScrolls[i];
                int basePrice = scrollId < 5 ? 40 : scrollId < 10 ? 50 : scrollId < 15 ? 70 : 100;
                int price = basePrice + Random.Range(0, 21) + (difficulty_dummy * 3);
                scrollButtons[i].gameObject.SetActive(true);
                scrollPriceTexts[i].text = $"스크롤 {scrollId} - {price}G";

                int buttonIndex = i;
                scrollButtons[i].onClick.RemoveAllListeners();
                scrollButtons[i].onClick.AddListener(() => BuyScroll(scrollId, price, scrollButtons[buttonIndex], scrollPriceTexts[buttonIndex]));
            }
            else
            {
                scrollButtons[i].gameObject.SetActive(false);
                scrollPriceTexts[i].text = "";
            }
        }
    }

    private void BuyMino(int minoId, int price, Button button, Text priceText)
    {
        if (InventoryManager.SpendGold(price))
        {
            InventoryManager.AddSpecialMino(minoId);
            button.interactable = false;
            priceText.text = "재고 없음";
        }
    }

    private void BuyScroll(int scrollId, int price, Button button, Text priceText)
    {
        if (InventoryManager.SpendGold(price))
        {
            InventoryManager.AddScroll(scrollId);
            button.gameObject.SetActive(false);
            priceText.text = "재고 없음";
        }
    }

    public void RefreshShop()
    {
        GenerateMinoShop();
        GenerateScrollShop();
    }

    public void ShopEnd()
    {
        Scene.Controller.OnClearScene();
        Debug.Log("상점방 종료 종료");
    }
    */
}