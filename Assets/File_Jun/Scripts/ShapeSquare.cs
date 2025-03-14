using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSquare : MonoBehaviour
{
    public Image occupiedImage;
    private bool hasCreatedHoldBlock = false;

    void Start()
    {
        occupiedImage.gameObject.SetActive(false);
    }

    public void DeactivateShape()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }

    public void ActivateShape()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasCreatedHoldBlock && other.CompareTag("Hold"))
        {
            hasCreatedHoldBlock = true;
            Debug.Log("HOLD 태그 충돌 감지! 블록 생성 시작");

            ShapeStorage shapeStorage = FindFirstObjectByType<ShapeStorage>();
            if (shapeStorage != null)
            {
                Shape selectedShape = shapeStorage.GetCurrentSelectedShape();
                if (selectedShape != null)
                {
                    Debug.Log($"현재 선택된 블록: {selectedShape.gameObject.name}");

                    foreach (Transform child in selectedShape.transform)
                    {
                        if (child.gameObject.activeSelf)
                        {
                            Debug.Log($"블록 비활성화: {child.gameObject.name}");
                            child.gameObject.SetActive(false);
                        }
                    }

                    HoldShape holdShape = FindFirstObjectByType<HoldShape>();
                    if (holdShape != null)
                    {
                        Debug.Log($"[{selectedShape.gameObject.name}] DeactivateShape() 실행 전 상태: {selectedShape.gameObject.activeSelf}");
                        selectedShape.DeactivateShape();
                        Debug.Log($"[{selectedShape.gameObject.name}] DeactivateShape() 실행 후 상태: {selectedShape.gameObject.activeSelf}");

                        holdShape.CreateShape(selectedShape.CurrentShapeData, selectedShape.CurrentShapeColorName);
                        Debug.Log($"HOLD 공간에 블록이 생성됨: {selectedShape.CurrentShapeColorName}");
                        Invoke(nameof(CheckAndRequestNewShapes), 0.5f);
                    }
                }
                else
                {
                    Debug.LogWarning("선택된 Shape이 없습니다!");
                }
            }
            else
            {
                Debug.LogWarning("ShapeStorage를 찾을 수 없습니다!");
            }
        }
    }



    private void CheckAndRequestNewShapes()
    {
        ShapeStorage shapeStorage = FindFirstObjectByType<ShapeStorage>();
        if (shapeStorage == null) return;

        var shapeLeft = shapeStorage.shapeList.Count(shape => shape.IsAnyOfShapeSquareActive());

        if (shapeLeft == 0)
        {
            Debug.Log("모든 블록이 사라졌음! 새로운 블록을 요청합니다.");
            GameEvents.RequestNewShapes();

            Grid grid = FindFirstObjectByType<Grid>();
            if (grid != null)
            {
                grid.enemies = grid.enemies.FindAll(enemy => enemy != null && enemy.GetComponent<EnemyStats>() != null);

                if (grid.enemies.Count > 0)
                {
                    Debug.Log($"[{grid.enemies.Count}]명의 적이 남아 있습니다. 공격을 실행합니다.");

                    foreach (var enemy in grid.enemies)
                    {
                        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                        if (enemyStats != null && enemyStats.GetCurrentHp() > 0)
                        {
                            enemyStats.PerformTurnAction(grid);
                            Debug.Log($"[{enemy.name}]이(가) 플레이어를 공격했습니다.");
                        }
                        else
                        {
                            Debug.Log($"[{enemy.name}]은(는) 이미 사망하여 공격하지 않습니다.");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("공격할 적이 없습니다.");
                }

                grid.CheckIfGameEnded();
            }
        }
    }
}
