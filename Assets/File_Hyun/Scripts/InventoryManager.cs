using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public static int Gold { get; private set; } = 0; // 보유 골드

    private const int BASIC_MINO_COUNT = 15;
    private const int SPECIAL_MINO_COUNT = 8;
    private const int MAGIC_SCROLL_COUNT = 17;

    public static List<bool> basicMinos = new();  // 기본 미노 (있으면 true, 없으면 false)
    public static List<bool> specialMinos = new(); // 특수 미노 (있으면 true, 없으면 false)
    public static List<int> magicScrolls = new();  // 마법 스크롤 (개수로 관리)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 기본 미노 초기화 (15개 모두 보유)
        for (int i = 0; i < BASIC_MINO_COUNT; i++)
            basicMinos.Add(true);

        // 특수 미노 초기화 (8개 모두 미보유)
        for (int i = 0; i < SPECIAL_MINO_COUNT; i++)
            specialMinos.Add(false);

        // 마법 스크롤 초기화 (17개 모두 0개)
        for (int i = 0; i < MAGIC_SCROLL_COUNT; i++)
            magicScrolls.Add(0);
    }

    // 골드 관리
    public static void AddGold(int amount) => Gold += amount;
    public static bool SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            return true;
        }
        return false;
    }

    // 기본 미노 관리
    public static void AddBasicMino(int number)
    {
        if (number >= 0 && number < BASIC_MINO_COUNT)
            basicMinos[number] = true;
    }

    public static bool RemoveBasicMino(int number)
    {
        if (number >= 0 && number < BASIC_MINO_COUNT && basicMinos[number])
        {
            basicMinos[number] = false;
            return true;
        }
        return false;
    }

    // 특수 미노 관리
    public static void AddSpecialMino(int number)
    {
        if (number >= 0 && number < SPECIAL_MINO_COUNT)
            specialMinos[number] = true;
    }

    public static bool RemoveSpecialMino(int number)
    {
        if (number >= 0 && number < SPECIAL_MINO_COUNT && specialMinos[number])
        {
            specialMinos[number] = false;
            return true;
        }
        return false;
    }

    // 마법 스크롤 관리
    public static void AddScroll(int number)
    {
        if (number >= 0 && number < MAGIC_SCROLL_COUNT)
            magicScrolls[number]++;
    }

    public static bool RemoveScroll(int number)
    {
        if (number >= 0 && number < MAGIC_SCROLL_COUNT && magicScrolls[number] > 0)
        {
            magicScrolls[number]--;
            return true;
        }
        return false;
    }
}