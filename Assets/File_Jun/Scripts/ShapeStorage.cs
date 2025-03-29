using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;
    public GameObject hold;

    private Dictionary<ShapeData, int> shapePool = new Dictionary<ShapeData, int>();
    private int totalShapesRemaining;

    void Start()
    {
        InitializeShapePool();
        DrawNewShapes();
    }

    private void OnEnable()
    {
        GameEvents.RequestNewShapes += RequestNewShapes;
    }

    private void OnDisable()
    {
        GameEvents.RequestNewShapes -= RequestNewShapes;
    }

    private void InitializeShapePool()
    {
        shapePool.Clear();
        totalShapesRemaining = 0;

        foreach (var shape in shapeData)
        {
            shapePool[shape] = 3;
            totalShapesRemaining += 3;
        }
    }

    private void DrawNewShapes()
    {
        if (totalShapesRemaining <= 0)
        {
            InitializeShapePool();
        }

        foreach (var shape in shapeList)
        {
            ShapeData selectedShape = GetRandomShape();
            if (selectedShape != null)
            {
                shape.RequestNewShape(selectedShape);

                int randomRotation = Random.Range(0, 4) * 90;
                Quaternion rotation = Quaternion.Euler(0, 0, randomRotation);
                shape.GetComponent<RectTransform>().rotation = rotation;

                SetHoldShape(selectedShape, shape.CurrentShapeColorName, rotation);
            }
        }
    }




    private ShapeData GetRandomShape()
    {
        List<ShapeData> availableShapes = new List<ShapeData>();

        foreach (var entry in shapePool)
        {
            if (entry.Value > 0) 
            {
                availableShapes.Add(entry.Key);
            }
        }

        if (availableShapes.Count == 0) return null;

        ShapeData chosenShape = availableShapes[Random.Range(0, availableShapes.Count)];
        shapePool[chosenShape]--;
        totalShapesRemaining--;

        return chosenShape;
    }

    public Shape GetCurrentSelectedShape()
    {
        foreach (var shape in shapeList)
        {
            if (shape.IsOnStartPosition() == false && shape.IsAnyOfShapeSquareActive())
                return shape;
        }
        return null;
    }

    public void RequestNewShapes()
    {
        DrawNewShapes();
    }

    public void SetHoldShape(ShapeData shapeData, string colorName, Quaternion rotation)
    {
        if (hold == null) return;

        HoldShape holdShape = hold.GetComponent<HoldShape>();
        if (holdShape != null)
        {
            holdShape.CreateShape(shapeData, colorName, rotation);
        }
    }

    public Shape GetRandomActiveShape()
    {
        List<Shape> activeShapes = shapeList.Where(shape => shape.gameObject.activeSelf).ToList();

        if (activeShapes.Count == 0)
            return null;

        return activeShapes[Random.Range(0, activeShapes.Count)];
    }

    public void RedrawOneShape()
    {
        var activeShapes = shapeList.Where(shape => shape.IsAnyOfShapeSquareActive()).ToList();

        if (activeShapes.Count == 0)
        {
            Debug.LogWarning("[RedrawOneShape] 다시 뽑을 수 있는 미노가 없습니다.");
            return;
        }

        int randomIndex = Random.Range(0, activeShapes.Count);
        Shape shapeToReplace = activeShapes[randomIndex];

        ShapeData newShape = GetRandomShape();
        if (newShape == null)
        {
            Debug.LogWarning("[RedrawOneShape] 새로운 ShapeData를 가져올 수 없습니다.");
            return;
        }

        int randomRotation = Random.Range(0, 4) * 90;
        Quaternion rotation = Quaternion.Euler(0, 0, randomRotation);

        shapeToReplace.RequestNewShape(newShape);
        shapeToReplace.GetComponent<RectTransform>().rotation = rotation;

        SetHoldShape(newShape, shapeToReplace.CurrentShapeColorName, rotation);

        Debug.Log($"[RedrawOneShape] {shapeToReplace.name} 미노가 새로 뽑힌 블록으로 교체됨");
    }



}
