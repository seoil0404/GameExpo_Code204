using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public static int Gold { get; private set; } = 0; // ���� ���

    private const int BASIC_MINO_COUNT = 15;
    private const int SPECIAL_MINO_COUNT = 8;
    private const int MAGIC_SCROLL_COUNT = 17;

    public static List<bool> basicMinos = new();  // �⺻ �̳� (������ true, ������ false)
    public static List<bool> specialMinos = new(); // Ư�� �̳� (������ true, ������ false)
    public static List<int> magicScrolls = new();  // ���� ��ũ�� (������ ����)

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

        // �⺻ �̳� �ʱ�ȭ (15�� ��� ����)
        for (int i = 0; i < BASIC_MINO_COUNT; i++)
            basicMinos.Add(true);

        // Ư�� �̳� �ʱ�ȭ (8�� ��� �̺���)
        for (int i = 0; i < SPECIAL_MINO_COUNT; i++)
            specialMinos.Add(false);

        // ���� ��ũ�� �ʱ�ȭ (17�� ��� 0��)
        for (int i = 0; i < MAGIC_SCROLL_COUNT; i++)
            magicScrolls.Add(0);
    }

    // ��� ����
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

    // �⺻ �̳� ����
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

    // Ư�� �̳� ����
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

    // ���� ��ũ�� ����
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