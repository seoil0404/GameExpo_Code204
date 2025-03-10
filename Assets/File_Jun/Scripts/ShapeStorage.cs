using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;
    public GameObject hold;

    void Start()
    {
        foreach (var shape in shapeList)
        {
            var shapeIndex = Random.Range(0, shapeData.Count);
            shape.CreateShape(shapeData[shapeIndex]);
        }
    }

    private void OnDisable()
    {
        GameEvents.RequestNewShapes -= RequestNewShapes;
    }

    private void OnEnable()
    {
        GameEvents.RequestNewShapes += RequestNewShapes;        
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

    private void RequestNewShapes()
    {
        foreach(var shape in shapeList)
        {
            var shapeIndex = Random.Range(0, shapeData.Count);
            shape.RequestNewShape(shapeData[shapeIndex]);
        }
    }

    public void SetHoldShape(ShapeData shapeData, string colorName)
    {
        if (hold == null) return;

        HoldShape holdShape = hold.GetComponent<HoldShape>();
        if (holdShape != null)
        {
            holdShape.CreateShape(shapeData, colorName);
        }
    }

}
