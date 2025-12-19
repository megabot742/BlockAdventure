using System.Collections.Generic;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    [SerializeField] private List<ShapeData> shapeData;
    [SerializeField] public List<Shape> shapesList;
    void OnEnable()
    {
        GameEvents.RequestNewShape += RequestNewShape;
    }
    void OnDisable()
    {
        GameEvents.RequestNewShape -= RequestNewShape;   
    }
    void Start()
    {
        foreach (var shape in shapesList)
        {
            var shapeIndex = Random.Range(0, shapeData.Count);
            shape.CreateShape(shapeData[shapeIndex]);
        }
    }
    private void RequestNewShape()
    {
        foreach (var shape in shapesList)
        {
            var shapeIndex = Random.Range(0, shapeData.Count);
            shape.RequestNewShape(shapeData[shapeIndex]);
        }
    }
    public Shape GetCurrentSelectedShape()
    {
        foreach (var shape in shapesList)
        {
            if (shape.IsOnStartPosition() == false && shape.IsAnyOfShapeSquareActive())
            {
                return shape;
            }
        }
        Debug.LogError("There are no shape selected");
        return null;
    }
}
