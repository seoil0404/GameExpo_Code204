using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldShape : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public List<GameObject> squareShapeImages;
    private List<GameObject> _currentHoldShape = new List<GameObject>();
    private ShapeData _heldShapeData;
    private string _heldShapeColorName = "default";

    private Vector3 _startPosition;
    private RectTransform _transform;
    private Canvas _canvas;
    private bool _shapeActive = true;

    public string HeldShapeColorName => _heldShapeColorName;

    public void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _startPosition = _transform.localPosition;
    }

    public void CreateShape(ShapeData shapeData, string colorName)
    {
        _heldShapeData = shapeData;

        // ���� ��� ����
        foreach (var block in _currentHoldShape)
        {
            Destroy(block);
        }
        _currentHoldShape.Clear();

        // ���� �´� ��� ã��
        GameObject selectedBlockPrefab = squareShapeImages.Find(prefab => prefab.name.ToLower() == colorName);

        if (selectedBlockPrefab == null)
        {
            Debug.LogWarning($"[HoldShape] �ش� ������ ����� ã�� �� ����: {colorName}, �⺻�� ���");
            selectedBlockPrefab = squareShapeImages[0];
        }

        _heldShapeColorName = selectedBlockPrefab.name.ToLower();

        // ��� ����
        int totalSquares = GetNumberOfSquares(_heldShapeData);
        for (int i = 0; i < totalSquares; i++)
        {
            GameObject newBlock = Instantiate(selectedBlockPrefab, transform);
            _currentHoldShape.Add(newBlock);
            newBlock.SetActive(true);
        }

        var squareRect = selectedBlockPrefab.GetComponent<RectTransform>();
        var moveDistance = new Vector2(
            squareRect.rect.width * squareRect.localScale.x,
            squareRect.rect.height * squareRect.localScale.y
        );

        int currentIndexInList = 0;
        for (var row = 0; row < _heldShapeData.rows; row++)
        {
            for (var column = 0; column < _heldShapeData.columns; column++)
            {
                if (_heldShapeData.board[row].column[column])
                {
                    _currentHoldShape[currentIndexInList].SetActive(true);
                    _currentHoldShape[currentIndexInList].GetComponent<RectTransform>().localPosition = new Vector2(
                        GetXPositionForShapeSquare(_heldShapeData, column, moveDistance),
                        GetYPositionForShapeSquare(_heldShapeData, row, moveDistance)
                    );
                    currentIndexInList++;
                }
            }
        }
    }

    // �巡�� �̺�Ʈ ó��
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = Vector3.one;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Canvas canvas = _canvas;

        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            Camera.main,
            out localMousePosition
        );

        _startPosition = rectTransform.localPosition - (Vector3)localMousePosition;
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
            _transform.localPosition = localMousePosition + (Vector2)_startPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
        CheckIfCanBePlaced();
    }

    private void CheckIfCanBePlaced()
    {
        List<GridSquare> detectedSquares = new List<GridSquare>();

        foreach (var block in _currentHoldShape)
        {
            Vector2 worldPosition = block.transform.position;
            Collider2D hit = Physics2D.OverlapCircle(worldPosition, 0.05f);

            if (hit != null)
            {
                GridSquare gridSquare = hit.GetComponent<GridSquare>();
                if (gridSquare != null && gridSquare.CanUseThisSquare())
                {
                    detectedSquares.Add(gridSquare);
                }
            }
        }

        if (detectedSquares.Count == _currentHoldShape.Count)
        {
            foreach (var gridSquare in detectedSquares)
            {
                gridSquare.PlaceShapeOnBoard(_heldShapeColorName);
            }
            StartCoroutine(ResetHoldShapeAfterPlacement());
            Debug.Log("[HoldShape] ����� ���������� ��ġ��!");
        }
        else
        {
            Debug.Log("[HoldShape] ��ġ ����! ���� ��ġ�� ����");
            StartCoroutine(MoveBackToStartPosition());
        }
    }

    // ��ġ �� Hold ��� ��Ȱ��ȭ �� ����ġ �̵�
    private IEnumerator ResetHoldShapeAfterPlacement()
    {
        yield return new WaitForSeconds(0.1f); // �ణ�� ���� �� ó��

        foreach (var square in _currentHoldShape)
        {
            square.SetActive(false);
        }

        _shapeActive = false;
        _transform.localPosition = _startPosition; // ���� ��ġ�� �̵�
    }

    // ���� �� �ε巴�� ���� ��ġ�� �̵�
    private IEnumerator MoveBackToStartPosition()
    {
        float elapsedTime = 0f;
        float duration = 0.2f;
        Vector3 startPos = _transform.localPosition;

        while (elapsedTime < duration)
        {
            _transform.localPosition = Vector3.Lerp(startPos, _startPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _transform.localPosition = _startPosition; // ����
    }

    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int number = 0;
        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if (active) number++;
            }
        }
        return number;
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        int middleIndex = (shapeData.columns - 1) / 2;
        return (column - middleIndex) * moveDistance.x;
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        int middleIndex = (shapeData.rows - 1) / 2;
        return -(row - middleIndex) * moveDistance.y;
    }
}
