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
            Debug.Log("HOLD �±� �浹 ����! ��� ���� ����");

            ShapeStorage shapeStorage = FindFirstObjectByType<ShapeStorage>();
            if (shapeStorage != null)
            {
                Shape selectedShape = shapeStorage.GetCurrentSelectedShape();
                if (selectedShape != null)
                {
                    HoldShape holdShape = FindFirstObjectByType<HoldShape>();
                    if (holdShape != null)
                    {
                        
                        holdShape.CreateShape(selectedShape.CurrentShapeData, selectedShape.CurrentShapeColorName);
                        Debug.Log($"HOLD ������ ����� ������: {selectedShape.CurrentShapeColorName}");
                    }
                }
            }
        }
    }

}
