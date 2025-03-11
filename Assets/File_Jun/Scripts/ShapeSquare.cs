using System.Collections;
using System.Collections.Generic;
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
                    HoldShape holdShape = FindFirstObjectByType<HoldShape>();
                    if (holdShape != null)
                    {
                        // 선택된 블록을 비활성화
                        selectedShape.DeactivateShape();

                        // HOLD 영역에 새로운 블록 생성
                        holdShape.CreateShape(selectedShape.CurrentShapeData, selectedShape.CurrentShapeColorName);

                        Debug.Log($"HOLD 공간에 블록이 생성됨: {selectedShape.CurrentShapeColorName}");

                        // Shape의 IsAnyOfShapeSquareActive() 실행
                        bool isAnyActive = selectedShape.IsAnyOfShapeSquareActive();
                        Debug.Log($"현재 Shape의 활성화된 블록 확인: {isAnyActive}");
                    }
                }
            }
        }
    }
}
