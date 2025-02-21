using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int columns = 8;
    public int rows = 8;

    public float squaresGap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public Button resetButton;

    public List<GameObject> enemies;
    public int allClearBonus = 100;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();
    private LineIndicator _lineIndicator;
    private GameObject selectedEnemy;
    private int comboCount = 0;

    public static Grid instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
    }

    void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
        resetButton.onClick.AddListener(ResetGrid);
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPositions();
    }

    private void SpawnGridSquares()
    {
        int square_index = 0;

        for (var row = 0; row < rows; ++row)
        {
            for (var column = 0; column < columns; ++column)
            {
                var square = Instantiate(gridSquare);
                _gridSquares.Add(square);
                square.GetComponent<GridSquare>().SquareIndex = square_index;
                square.transform.SetParent(this.transform);
                square.transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                square.GetComponent<GridSquare>().SetImage(_lineIndicator.GetGridSquareIndex(square_index) % 2 == 0);
                square_index++;
            }
        }
    }

    private void SetGridSquaresPositions()
    {
        int column_number = 0;
        int row_number = 0;

        var square_rect = _gridSquares[0].GetComponent<RectTransform>();
        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + squaresGap;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + squaresGap;

        foreach (GameObject square in _gridSquares)
        {
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                startPosition.x + (column_number * _offset.x),
                startPosition.y - (row_number * _offset.y)
            );

            column_number++;
            if (column_number >= columns)
            {
                column_number = 0;
                row_number++;
            }
        }
    }

    private void CheckIfShapeCanBePlaced()
    {
        var squareIndexes = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();

            if (gridSquare.Selected && !gridSquare.SquareOccupied)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
            }
        }

        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return;

        if (currentSelectedShape.TotalSquareNumber == squareIndexes.Count)
        {
            foreach (var squareIndex in squareIndexes)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard();
            }

            GameEvents.SetShapeInactive();
            CheckIfAnyLineIsCompleted();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }

        var shapeLeft = shapeStorage.shapeList.Count(shape => shape.IsAnyOfShapeSquareActive());
        Debug.Log($"���� ��� ����: {shapeLeft}");

        if (shapeLeft == 0)
        {
            Debug.Log("��� ����� ��ġ �Ϸ�! ���ο� ����� �����մϴ�.");
            GameEvents.RequestNewShapes();


            enemies = enemies.Where(enemy => enemy != null && enemy.GetComponent<EnemyStats>() != null).ToList();

            if (enemies.Count > 0)
            {
                Debug.Log($"[CheckIfShapeCanBePlaced] ����� ��� ��ġ �Ϸ��. ���� ���� ����: {enemies.Count}");

                foreach (var enemy in enemies)
                {
                    var enemyStats = enemy.GetComponent<EnemyStats>();
                    if (enemyStats != null && enemyStats.GetCurrentHp() > 0)
                    {
                        enemyStats.AttackPlayer();
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
                Debug.LogWarning("[CheckIfShapeCanBePlaced] ������ ���� �����ϴ�.");
            }
        }

        CheckIfGameEnded();
    }



    private void CheckIfAnyLineIsCompleted()
    {
        List<int[]> lines = _lineIndicator.columnIndexes.Select(column => _lineIndicator.GetVerticalLine(column)).ToList();

        for (var row = 0; row < rows; row++)
        {
            lines.Add(Enumerable.Range(0, columns).Select(index => _lineIndicator.line_data[row, index]).ToArray());
        }

        var completedLines = CheckIfSquaresAreCompleted(lines);

        if (completedLines > 0)
        {
            DealDamageToSelectedEnemy(completedLines);
            comboCount++;
        }
        else
        {
            comboCount = 0;
        }

        CheckIfGameEnded();
    }

    private int CheckIfSquaresAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();
        var linesCompleted = 0;

        foreach (var line in data)
        {
            bool lineCompleted = true;
            foreach (var squareIndex in line)
            {
                if (!_gridSquares[squareIndex].GetComponent<GridSquare>().SquareOccupied)
                {
                    lineCompleted = false;
                    break;
                }
            }

            if (lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach (var line in completedLines)
        {
            MinoEffectHelper.Instance.PlayMinoEffect(_gridSquares, line);
            foreach (var squareIndex in line)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().ClearOccupied();
            }
            linesCompleted++;
        }

        return linesCompleted;
    }

    private void DealDamageToSelectedEnemy(int completedLines)
    {
        if (selectedEnemy == null)
        {
            Debug.Log("���õ� ���� �����ϴ�.");
            return;
        }

        var enemyStats = selectedEnemy.GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("���� EnemyStats�� ã�� �� �����ϴ�.");
            return;
        }

        enemyStats.ReceiveDamage(completedLines, columns);
    }


    public void CheckIfGameEnded()
    {
        if (enemies == null)
        {
            Debug.LogWarning("enemies ����Ʈ�� `null` �����Դϴ�. ���� ���� üũ�� ���� �ʽ��ϴ�.");
            return;
        }
        bool allEnemiesDefeated = enemies.All(enemy => enemy.GetComponent<EnemyStats>().GetCurrentHp() <= 0);

        if (allEnemiesDefeated)
        {
            Debug.Log(" ��� ���� óġ�Ǿ����ϴ�. ���� ���������� �̵��մϴ�.");
            FindFirstObjectByType<EnemySpawner>().IncreaseDifficulty();
            MoveNextScene();
        }
    }




    public void MoveNextScene()
    {
        Scene.Controller.OnClearScene();
    }


    public void ResetGrid()
    {
        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            gridSquare.ClearOccupied();
            gridSquare.Deactivate();
        }

        Debug.Log("�׸��尡 ���µǾ����ϴ�.");
        comboCount--;
        CheckIfGameEnded();
    }

    public void SelectEnemy(GameObject enemy)
    {
        selectedEnemy = enemy;
        Debug.Log($"[{selectedEnemy.name}]��(��) �����߽��ϴ�.");
    }

    public void DestroyRandomPlayerBlock()
    {
        List<GameObject> playerBlocks = _gridSquares.Where(sq => sq.GetComponent<GridSquare>().SquareOccupied).ToList();

        if (playerBlocks.Count > 0)
        {
            GameObject blockToDestroy = playerBlocks[Random.Range(0, playerBlocks.Count)];
            blockToDestroy.GetComponent<GridSquare>().ClearOccupied();
            Debug.Log("�÷��̾� ��� �ϳ��� ���ŵǾ����ϴ�.");
        }
    }

    public void SpawnRandomBlock()
    {
        int randomIndex = Random.Range(0, _gridSquares.Count);
        _gridSquares[randomIndex].GetComponent<GridSquare>().SetOccupied();
        Debug.Log("������ ��ġ�� ����� �����Ǿ����ϴ�.");
    }

    public void SealRandomBlock(GameObject enemy)
    {
        List<GameObject> playerBlocks = _gridSquares.Where(sq => sq.GetComponent<GridSquare>().SquareOccupied).ToList();

        if (playerBlocks.Count > 0)
        {
            GameObject blockToSeal = playerBlocks[Random.Range(0, playerBlocks.Count)];
            blockToSeal.GetComponent<GridSquare>().SealBlock(enemy);
            Debug.Log($"{enemy.name}��(��) �÷��̾� ����� �����߽��ϴ�.");
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"[{enemy.name}]��(��) ����Ʈ���� ���ŵǾ����ϴ�.");
        }
        else
        {
            Debug.LogWarning($"[{enemy.name}]��(��) ����Ʈ���� ã�� �� �����ϴ�.");
        }
    }



    public GameObject GetSelectedEnemy()
    {
        return selectedEnemy;
    }
}
