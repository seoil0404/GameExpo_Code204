using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Shape : MonoBehaviour , IPointerClickHandler , IPointerUpHandler , IBeginDragHandler , IDragHandler , IEndDragHandler , IPointerDownHandler
{
    public List<GameObject> squareShapeImages;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2(0f, 700f);

    [HideInInspector]
    public ShapeData CurrentShapeData;
    public int TotalSquareNumber { get; set; }
    private List<GameObject> _currentShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private RectTransform _transform;
    private Canvas _canvas;
    private Vector3 _startPosition;
    private bool _shapeActive = true;


     
    public void Awake()
    {
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _startPosition = _transform.localPosition;
    }

    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvents.SetShapeInactive -= SetShapeInactive; 
    }

    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEvents.SetShapeInactive += SetShapeInactive;    
    }


    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }

    public bool IsAnyOfShapeSquareActive()
    {
        foreach (var square in _currentShape)
        {
            if (square.gameObject.activeSelf)
                return true;
        }
        return false;
    }

    private void SetShapeInactive()
    {
        if(IsOnStartPosition() == false && IsAnyOfShapeSquareActive())
        {
            foreach(var square in _currentShape)
            {
                square.gameObject.SetActive(false);
            }
        } 
    }
    public void DeactivateShape()
    {
        if (_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().DeactivateShape();
            }
            _shapeActive = false;
        }
    }

    public void ActivateShape()
    {
        if (!_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().ActivateShape();
            }
            _shapeActive = true;
        }
    }


    void Start()
    {
        
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        _transform.localPosition = _startPosition;
        CreateShape(shapeData);
    }

    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int number = 0;
        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if (active)
                    number++;
            }
        }
        return number;
    }

    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        TotalSquareNumber = GetNumberOfSquares(shapeData);

      
        foreach (var block in _currentShape)
        {
            Destroy(block);
        }
        _currentShape.Clear();

       
        GameObject selectedBlockPrefab = squareShapeImages[Random.Range(0, squareShapeImages.Count)];

       
        for (int i = 0; i < TotalSquareNumber; i++)
        {
            GameObject newBlock = Instantiate(selectedBlockPrefab, transform);
            _currentShape.Add(newBlock);
            newBlock.SetActive(false);
        }

    
        var squareRect = selectedBlockPrefab.GetComponent<RectTransform>();
        var moveDistance = new Vector2(
            squareRect.rect.width * squareRect.localScale.x,
            squareRect.rect.height * squareRect.localScale.y
        );

        int currentIndexInList = 0;
        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var column = 0; column < shapeData.columns; column++)
            {
                if (shapeData.board[row].column[column])
                {
                    _currentShape[currentIndexInList].SetActive(true);
                    _currentShape[currentIndexInList].GetComponent<RectTransform>().localPosition = new Vector2(
                        GetXPositionForShapeSquare(shapeData, column, moveDistance),
                        GetYPositionForShapeSquare(shapeData, row, moveDistance)
                    );
                    currentIndexInList++;
                }
            }
        }
    }

    public float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
       
        int middleIndex = (shapeData.columns - 1) / 2;
        float shiftX = (column - middleIndex) * moveDistance.x;
        return shiftX;
    }


    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        
        int middleIndex = (shapeData.rows - 1) / 2;
        float shiftY = -(row - middleIndex) * moveDistance.y;
        return shiftY;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
       
        this.GetComponent<RectTransform>().localScale = Vector3.one;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Canvas canvas = _canvas;

        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            Camera.main,
            out localMousePosition
        );

        offset = (Vector2)rectTransform.localPosition - localMousePosition;
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localMousePosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                eventData.position,
                Camera.main,
                out localMousePosition))
        {
            
            _transform.localPosition = localMousePosition + offset;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvents.CheckIfShapeCanBePlaced();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    private void MoveShapeToStartPosition()
    {
        _transform.transform.localPosition = _startPosition;
    }
   

}
