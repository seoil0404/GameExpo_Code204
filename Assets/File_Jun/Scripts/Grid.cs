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

    // ★ 궁극기 효과 관련 필드
    public float ultimateDamageMultiplier = 1f;
    public int additionalExecutionDamage = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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
        if (currentSelectedShape == null)
            return;
        if (currentSelectedShape.TotalSquareNumber == squareIndexes.Count)
        {
            string shapeColorName = currentSelectedShape.CurrentShapeColorName;
            foreach (var squareIndex in squareIndexes)
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard(shapeColorName);
            GameEvents.SetShapeInactive();
            CheckIfAnyLineIsCompleted();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }
        var shapeLeft = shapeStorage.shapeList.Count(shape => shape.IsAnyOfShapeSquareActive());
        Debug.Log($"남은 블록 개수: {shapeLeft}");
        if (shapeLeft == 0)
        {
            Debug.Log("모든 블록이 배치 완료! 새로운 블록을 생성합니다.");
            GameEvents.RequestNewShapes();
            enemies = enemies.Where(enemy => enemy != null && enemy.GetComponent<EnemyStats>() != null).ToList();
            if (enemies.Count > 0)
            {
                Debug.Log($"[CheckIfShapeCanBePlaced] 블록이 모두 배치 완료됨. 현재 적의 개수: {enemies.Count}");
                foreach (var enemy in enemies)
                {
                    var enemyStats = enemy.GetComponent<EnemyStats>();
                    if (enemyStats != null && enemyStats.GetCurrentHp() > 0)
                    {
                        enemyStats.PerformTurnAction(this);
                        Debug.Log($"[{enemy.name}]이(가) 플레이어를 공격했습니다.");
                    }
                    else
                    {
                        Debug.Log($"[{enemy.name}]은(는) 이미 사망하여 공격하지 않습니다.");
                    }
                }
            }
            else
            {
                Debug.LogWarning("[CheckIfShapeCanBePlaced] 공격할 적이 없습니다.");
            }
        }
        CheckIfGameEnded();
    }

    private void CheckIfAnyLineIsCompleted()
    {
        List<int[]> lines = _lineIndicator.columnIndexes.Select(column => _lineIndicator.GetVerticalLine(column)).ToList();
        for (var row = 0; row < rows; row++)
            lines.Add(Enumerable.Range(0, columns).Select(index => _lineIndicator.line_data[row, index]).ToArray());
        var completedLines = CheckIfSquaresAreCompleted(lines);
        if (completedLines > 0)
        {
            DealDamageToSelectedEnemy(completedLines);
            comboCount++;
            // 부서진 줄 하나당 궁극기 게이지 1씩 증가
            CharacterManager.selectedCharacter.characterData.CurrentUltimateGauge += completedLines;
            CharacterManager.SaveUltimateGauge();
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
        int linesCompleted = 0;
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
                completedLines.Add(line);
        }
        foreach (var line in completedLines)
        {
            MinoEffectHelper.Instance.PlayMinoEffect(_gridSquares, line);
            foreach (var squareIndex in line)
                _gridSquares[squareIndex].GetComponent<GridSquare>().ClearOccupied();
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

        int baseDamage = completedLines * columns;
        baseDamage = (int)(baseDamage * ultimateDamageMultiplier);
        baseDamage += additionalExecutionDamage;

        // multiplier와 추가 데미지 초기화
        ultimateDamageMultiplier = 1f;
        additionalExecutionDamage = 0;

        // enemyStats.ReceiveDamage는 두 개의 인자를 받도록 정의되어 있으므로,
        // baseDamage와 columns를 함께 전달합니다.
        enemyStats.ReceiveDamage(baseDamage, columns);
        Debug.Log($"최종 데미지: {baseDamage} (클리어 줄: {completedLines})");
    }


    public void CheckIfGameEnded()
    {
        if (enemies == null)
        {
            Debug.LogWarning("enemies 리스트가 `null` 상태입니다. 게임 종료 체크를 하지 않습니다.");
            return;
        }
        bool allEnemiesDefeated = enemies.All(enemy => enemy.GetComponent<EnemyStats>().GetCurrentHp() <= 0);
        if (allEnemiesDefeated)
        {
            Debug.Log(" 모든 적이 처치되었습니다. 다음 스테이지로 이동합니다.");
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
        foreach (var enemy in enemies)
        {
            var enemyStats = enemy.GetComponent<EnemyStats>();
            if (enemyStats != null && enemyStats.GetCurrentHp() > 0)
            {
                enemyStats.PerformTurnAction(this);
                Debug.Log($"[{enemy.name}]이(가) 플레이어를 공격했습니다.");
            }
            else
            {
                Debug.Log($"[{enemy.name}]은(는) 이미 사망하여 공격하지 않습니다.");
            }
        }
        Debug.Log("그리드가 리셋되었습니다.");
        comboCount--;
        CheckIfGameEnded();
    }

    public void DestroyRandomPlayerBlock()
    {
        List<GameObject> playerBlocks = _gridSquares
            .Where(sq => sq.GetComponent<GridSquare>().SquareOccupied)
            .ToList();
        if (playerBlocks.Count > 0)
        {
            GameObject blockToDestroy = playerBlocks[Random.Range(0, playerBlocks.Count)];
            GridSquare gs = blockToDestroy.GetComponent<GridSquare>();
            MinoEffectHelper.Instance.PlayMinoEffectSingle(blockToDestroy);
            gs.ClearOccupied();
            gs.Deactivate();
            Debug.Log("플레이어 블록 하나가 제거되었습니다.");
        }
    }

    public void SpawnRandomBlock()
    {
        if (_gridSquares.Count == 0)
        {
            Debug.LogWarning("그리드에 블록을 생성할 공간이 없습니다.");
            return;
        }
        List<GridSquare> emptySquares = _gridSquares
            .Select(sq => sq.GetComponent<GridSquare>())
            .Where(sq => !sq.SquareOccupied)
            .ToList();
        if (emptySquares.Count == 0)
        {
            Debug.LogWarning("모든 칸이 차 있어서 블록을 생성할 공간이 없습니다.");
            return;
        }
        int randomIndex = Random.Range(0, emptySquares.Count);
        GridSquare gs = emptySquares[randomIndex];
        gs.SetOccupied();
        gs.ActivateSquare();
        gs.SetBlockSpriteToDefault();
        Debug.Log($"블록이 랜덤한 위치({gs.SquareIndex})에 DefaultSprite로 생성되었습니다.");
        CheckIfAnyLineIsCompleted();
    }

    public void SelectEnemy(GameObject enemy)
    {
		CharacterManager.selectedCharacter.characterData.AttackEffectSpawner.TargetTransform = enemy.transform;
        selectedEnemy = enemy;
        Debug.Log($"[{selectedEnemy.name}]을(를) 선택했습니다.");
    }

    public void DeactivateRandom4x4()
    {
        if (_gridSquares.Count == 0)
        {
            Debug.LogWarning("그리드가 비어 있어 블록을 제거할 수 없습니다.");
            return;
        }
        int gridSize = 8;
        int x = Random.Range(0, gridSize - 3);
        int y = Random.Range(0, gridSize - 3);
        Debug.Log($"[{x}, {y}] 위치에서 4x4 블록을 비활성화합니다.");
        for (int i = x; i < x + 4; i++)
        {
            for (int j = y; j < y + 4; j++)
            {
                int index = j * gridSize + i;
                if (index < _gridSquares.Count)
                    _gridSquares[index].GetComponent<GridSquare>().DeactivateBlock();
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"[{enemy.name}]이(가) 리스트에서 제거되었습니다.");
        }
        else
        {
            Debug.LogWarning($"[{enemy.name}]을(를) 리스트에서 찾을 수 없습니다.");
        }
    }

    public GameObject GetSelectedEnemy()
    {
        return selectedEnemy;
    }

    

    // Grid.cs 내에 추가할 메서드
    public void DropAllBlocks()
    {
        for (int col = 0; col < columns; col++)
        {
            List<GridSquare> occupiedSquares = new List<GridSquare>();
            // 해당 열의 모든 행을 순회하며 점유된 블록 수집
            for (int row = 0; row < rows; row++)
            {
                int index = row * columns + col;
                GridSquare gs = _gridSquares[index].GetComponent<GridSquare>();
                if (gs.SquareOccupied)
                {
                    occupiedSquares.Add(gs);
                }
            }
            // 해당 열의 모든 칸 초기화
            for (int row = 0; row < rows; row++)
            {
                int index = row * columns + col;
                GridSquare gs = _gridSquares[index].GetComponent<GridSquare>();
                gs.ClearOccupied();
                gs.Deactivate();
            }
            // 수집한 블록들을 아래쪽(높은 row)부터 재배치
            int startRow = rows - occupiedSquares.Count;
            for (int i = 0; i < occupiedSquares.Count; i++)
            {
                int row = startRow + i;
                int index = row * columns + col;
                GridSquare gs = _gridSquares[index].GetComponent<GridSquare>();
                gs.SetOccupied();
                gs.ActivateSquare();
                gs.SetBlockSpriteToDefault();
            }
        }
        Debug.Log("DropAllBlocks: 모든 블록이 아래로 떨어졌습니다.");
    }
}
