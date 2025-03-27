using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNextAction : MonoBehaviour
{
    public Image[] actionImages;  // �ൿ �̹��� �迭 (�����տ��� ����)
    public Text damageText;       // ���� ���� ���� ������ ǥ�� �ؽ�Ʈ
    private int nextActionIndex = 1; // ���� �ൿ (1 = ����, 2 �̻� = ��ų)

    public void DecideNextAction(int totalOptions, int attackDamage)
    {
        nextActionIndex = Random.Range(1, totalOptions + 1);

        // **��� �̹��� ��Ȱ��ȭ ��, ���� ���� �ش� �ൿ�� Ȱ��ȭ**
        for (int i = 0; i < actionImages.Length; i++)
        {
            actionImages[i].gameObject.SetActive(i == nextActionIndex - 1);
        }

        // **�� ������ ���� UI�� �����ϵ��� ����**
        if (nextActionIndex == 1)
        {
            damageText.text = attackDamage.ToString();
            damageText.gameObject.SetActive(true);
        }
        else
        {
            damageText.gameObject.SetActive(false);
        }

        Debug.Log($"[EnemyNextAction] {gameObject.name}�� ���� �ൿ ������: {nextActionIndex}");
    }

    public int GetNextActionIndex()
    {
        return nextActionIndex;
    }

    public void HideAllActionIndicators()
    {
        foreach (var image in actionImages)
        {
            image.gameObject.SetActive(false);
        }

        if (damageText != null)
        {
            damageText.gameObject.SetActive(false);
        }

        Debug.Log($"[EnemyNextAction] {gameObject.name}�� �ൿ ǥ�� ��Ȱ��ȭ �Ϸ�");
    }

}


