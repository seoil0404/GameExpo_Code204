using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNextAction : MonoBehaviour
{
    public Image[] actionImages;  // 행동 이미지 배열 (프리팹에서 설정)
    public Text damageText;       // 개별 적의 공격 데미지 표시 텍스트
    private int nextActionIndex = 1; // 다음 행동 (1 = 공격, 2 이상 = 스킬)

    public void DecideNextAction(int totalOptions, int attackDamage)
    {
        nextActionIndex = Random.Range(1, totalOptions + 1);

        // **모든 이미지 비활성화 후, 현재 적의 해당 행동만 활성화**
        for (int i = 0; i < actionImages.Length; i++)
        {
            actionImages[i].gameObject.SetActive(i == nextActionIndex - 1);
        }

        // **각 적별로 개별 UI를 유지하도록 보장**
        if (nextActionIndex == 1)
        {
            damageText.text = attackDamage.ToString();
            damageText.gameObject.SetActive(true);
        }
        else
        {
            damageText.gameObject.SetActive(false);
        }

        Debug.Log($"[EnemyNextAction] {gameObject.name}의 다음 행동 결정됨: {nextActionIndex}");
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

        Debug.Log($"[EnemyNextAction] {gameObject.name}의 행동 표시 비활성화 완료");
    }

}


