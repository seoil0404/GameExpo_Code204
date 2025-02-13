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
    public Text winText;
    public Text loseText;

    public List<GameObject> enemies;
    public int allClearBonus = 100;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();
    private LineIndicator _lineIndicator;
    private GameObject selectedEnemy;
    private int comboCount = 0;

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

        var shapeLeft = 0;
        foreach (var shape in shapeStorage.shapeList)
        {
            if (shape.IsAnyOfShapeSquareActive())
            {
                shapeLeft++;
            }
        }

        Debug.Log($"남은 블록 개수: {shapeLeft}");

        if (shapeLeft == 0)
        {
            Debug.Log("모든 블록이 배치 완료! 새로운 블록을 생성합니다.");
            GameEvents.RequestNewShapes();
        }

        CheckIfGameEnded();
    }

    private void CheckIfAnyLineIsCompleted()
    {
        List<int[]> lines = new List<int[]>();

        foreach (var column in _lineIndicator.columnIndexes)
        {
            lines.Add(_lineIndicator.GetVerticalLine(column));
        }

        for (var row = 0; row < rows; row++)
        {
            List<int> data = new List<int>(columns);
            for (var index = 0; index < columns; index++)
            {
                data.Add(_lineIndicator.line_data[row, index]);
            }
            lines.Add(data.ToArray());
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
			MinoEffectHelper.Instance.PlayMinoEffect(
				_gridSquares,
				line
			);

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
            Debug.Log("선택된 적이 없습니다.");
            return;
        }

        var enemyStats = selectedEnemy.GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("적의 EnemyStats를 찾을 수 없습니다.");
            return;
        }

        float dodgeChance = enemyStats.GetDodgeChance();
        int dodgeRoll = Random.Range(0, 100);

        Debug.Log($" 회피 체크: 랜덤값({dodgeRoll}) vs 회피 확률({dodgeChance}%)");

        if (dodgeRoll < dodgeChance)
        {
            Debug.Log($" [{selectedEnemy.name}]이(가) 공격을 회피했습니다! 데미지를 받지 않습니다.");
            return;
        }

        int totalBlocksUsed = completedLines * columns;
        string damageSkin = DetermineDamageSkin();
        bool isAllClear = IsAllClear();
        int baseDamage = totalBlocksUsed;
        int calculatedDamage = isAllClear ? (baseDamage * 2) * (comboCount * 2) : baseDamage + (comboCount * 2);

        enemyStats.TakeDamage(calculatedDamage);
        Debug.Log($" [{selectedEnemy.name}]에게 {calculatedDamage} 데미지를 입혔습니다. (데미지 스킨: {damageSkin})");

        if (isAllClear)
        {
            Debug.Log("판이 완전히 클리어됨! 액티브 스킬 게이지 100% 충전");
        }

        comboCount++;
    }


    public void DestroyRandomPlayerBlock()
    {
        List<GameObject> playerBlocks = _gridSquares.Where(sq => sq.GetComponent<GridSquare>().SquareOccupied).ToList();

        if (playerBlocks.Count > 0)
        {
            GameObject blockToDestroy = playerBlocks[Random.Range(0, playerBlocks.Count)];
            blockToDestroy.GetComponent<GridSquare>().ClearOccupied();
        }
    }


    private string DetermineDamageSkin()
    {
        Dictionary<string, int> colorCount = new Dictionary<string, int>();

        foreach (var square in _gridSquares)
        {
            if (square.GetComponent<GridSquare>().SquareOccupied)
            {
                string color = square.GetComponent<GridSquare>().GetBlockColor();
                if (colorCount.ContainsKey(color))
                    colorCount[color]++;
                else
                    colorCount[color] = 1;
            }
        }

        string maxColor = "기본";
        int maxCount = 0;
        foreach (var kvp in colorCount)
        {
            if (kvp.Value > maxCount)
            {
                maxCount = kvp.Value;
                maxColor = kvp.Key;
            }
        }

        return maxColor;
    }


    public void SpawnRandomBlock()
    {
        int randomIndex = Random.Range(0, _gridSquares.Count);
        _gridSquares[randomIndex].GetComponent<GridSquare>().SetOccupied();
    }


    public void SealRandomBlock(GameObject enemy)
    {
        List<GameObject> playerBlocks = _gridSquares.Where(sq => sq.GetComponent<GridSquare>().SquareOccupied).ToList();

        if (playerBlocks.Count > 0)
        {
            GameObject blockToSeal = playerBlocks[Random.Range(0, playerBlocks.Count)];
            blockToSeal.GetComponent<GridSquare>().SealBlock(enemy);
        }
    }



    private void CheckIfGameEnded()
    {
        var characterManager = FindAnyObjectByType<CharacterManager>();
        if (characterManager != null && characterManager.GetCurrentHp() <= 0)
        {
            Debug.Log("플레이어 체력이 0이 되어 게임이 종료되었습니다!");

        }

		#pragma warning disable CS0219 // To 정준. 이거 경고 뜨는것 때문에 달아놨다. 안쓰는 변수는 지우고 올리세요. 쓸때 이거 지우고 써
		bool allEnemiesDefeated = true;
		#pragma warning restore CS0219

		foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<EnemyStats>().GetCurrentHp() > 0)
            {
                allEnemiesDefeated = false;
                break;
            }
        }
    }

    private bool IsAllClear()
    {
        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.SquareOccupied)
            {
                return false;
            }
        }
        return true;
    }

    public void ResetGrid()
    {
        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            gridSquare.ClearOccupied();
            gridSquare.Deactivate();
        }

        Debug.Log("그리드가 리셋되었습니다.");
        CheckIfGameEnded();
    }

    public void SelectEnemy(GameObject enemy)
    {
        selectedEnemy = enemy;
        Debug.Log($"[{selectedEnemy.name}]을(를) 선택했습니다.");
    }


    public GameObject GetSelectedEnemy()
    {
        return selectedEnemy;
    }

}
