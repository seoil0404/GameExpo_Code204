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
            Debug.Log("HOLD �±� �浹 ����! ��� ���� ����");

            ShapeStorage shapeStorage = FindFirstObjectByType<ShapeStorage>();
            if (shapeStorage != null)
            {
                Shape selectedShape = shapeStorage.GetCurrentSelectedShape();
                if (selectedShape != null)
                {
                    Debug.Log($"���� ���õ� ���: {selectedShape.gameObject.name}");

                    foreach (Transform child in selectedShape.transform)
                    {
                        if (child.gameObject.activeSelf)
                        {
                            Debug.Log($"��� ��Ȱ��ȭ: {child.gameObject.name}");
                            child.gameObject.SetActive(false);
                        }
                    }

                    HoldShape holdShape = FindFirstObjectByType<HoldShape>();
                    if (holdShape != null)
                    {
                        Debug.Log($"[{selectedShape.gameObject.name}] DeactivateShape() ���� �� ����: {selectedShape.gameObject.activeSelf}");
                        selectedShape.DeactivateShape();
                        Debug.Log($"[{selectedShape.gameObject.name}] DeactivateShape() ���� �� ����: {selectedShape.gameObject.activeSelf}");

                        holdShape.CreateShape(selectedShape.CurrentShapeData, selectedShape.CurrentShapeColorName);
                        Debug.Log($"HOLD ������ ����� ������: {selectedShape.CurrentShapeColorName}");
                        Invoke(nameof(CheckAndRequestNewShapes), 0.5f);
                    }
                }
                else
                {
                    Debug.LogWarning("���õ� Shape�� �����ϴ�!");
                }
            }
            else
            {
                Debug.LogWarning("ShapeStorage�� ã�� �� �����ϴ�!");
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
            Debug.Log("��� ����� �������! ���ο� ����� ��û�մϴ�.");
            GameEvents.RequestNewShapes();

            Grid grid = FindFirstObjectByType<Grid>();
            if (grid != null)
            {
                grid.enemies = grid.enemies.FindAll(enemy => enemy != null && enemy.GetComponent<EnemyStats>() != null);

                if (grid.enemies.Count > 0)
                {
                    Debug.Log($"[{grid.enemies.Count}]���� ���� ���� �ֽ��ϴ�. ������ �����մϴ�.");

                    foreach (var enemy in grid.enemies)
                    {
                        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                        if (enemyStats != null && enemyStats.GetCurrentHp() > 0)
                        {
                            enemyStats.PerformTurnAction(grid);
                            Debug.Log($"[{enemy.name}]��(��) �÷��̾ �����߽��ϴ�.");
                        }
                        else
                        {
                            Debug.Log($"[{enemy.name}]��(��) �̹� ����Ͽ� �������� �ʽ��ϴ�.");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("������ ���� �����ϴ�.");
                }

                grid.CheckIfGameEnded();
            }
        }
    }
}
