using System.Collections;
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

    public float ultimateDamageMultiplier = 1f;
    public int additionalExecutionDamage = 0;

    private int turnCounter = 0;

    [SerializeField] private GameObject rewardsScreen;

    [SerializeField] private AudioClip placeMino;
    [SerializeField] private AudioClip minoDel;
    private AudioSource place;
    private AudioSource del;

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
        place = gameObject.AddComponent<AudioSource>();
        place.clip = placeMino;
        del = gameObject.AddComponent<AudioSource>();
        del.clip = minoDel;

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
            place.Play();
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
            foreach (var square in _gridSquares)
            {
                var gs = square.GetComponent<GridSquare>();
                gs.ClearPetrified();
            }

            var treasureEffect = FindFirstObjectByType<TreasureEffect>();
            if (treasureEffect != null && treasureEffect.TalismanOfPower)
            {
                turnCounter++;

                if (turnCounter % 2 == 0)
                {
                    CharacterManager.selectedCharacter.characterData.CurrentCharacterATK += 1;
                    Debug.Log("[TalismanOfPower] 2�� ���! ĳ���� ATK +2 ����");
                }
            }

            if (treasureEffect != null && treasureEffect.CorruptTouch)
            {
                GameObject selectedEnemy = GetSelectedEnemy();
                if (selectedEnemy != null)
                {
                    EnemyStats enemyStats = selectedEnemy.GetComponent<EnemyStats>();
                    if (enemyStats != null && enemyStats.GetCurrentHp() > 0)
                    {
                        enemyStats.ReceiveDamage(1, columns);
                        EffectManager.Instance.OnPoison(selectedEnemy);
                        Debug.Log("[CorruptTouch] ���õ� ���� HP�� 1 ���ҵ�");
                    }
                }
            }

            foreach (var enemy in enemies)
            {
                var enemyStats = enemy.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.DeactivateDamageMultiplier();
                    enemyStats.ResetThorn();
                    enemyStats.TickHealingReduction();

                }
            }

            HoldShape holdShape = FindFirstObjectByType<HoldShape>();
            if (holdShape != null)
            {
                holdShape.RestoreAssignedObject();
                holdShape.UnlockShape();
            }

            StartCoroutine(EnemyTurnSequence());
        }
        CheckIfGameEnded();
    }

    public void CheckIfAnyLineIsCompleted()
    {
        List<int[]> lines = _lineIndicator.columnIndexes.Select(column => _lineIndicator.GetVerticalLine(column)).ToList();
        for (var row = 0; row < rows; row++)
            lines.Add(Enumerable.Range(0, columns).Select(index => _lineIndicator.line_data[row, index]).ToArray());
        var completedLines = CheckIfSquaresAreCompleted(lines);
        if (completedLines > 0)
        {
            DealDamageToSelectedEnemy(completedLines);
            comboCount++;
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
                var gs = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (!gs.SquareOccupied || gs.IsPetrified())
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
            {
                var gs = _gridSquares[squareIndex].GetComponent<GridSquare>();

                GameObject owner = gs.GetSpecialMinoOwner();
                if (owner != null)
                {
                    var stats = owner.GetComponent<EnemyStats>();
                    if (stats != null)
                    {
                        stats.IncreaseATKByTwo();
                    }
                    gs.ClearSpecialMinoOwner();
                }

                GameObject lifestealOwner = gs.GetLifestealMinoOwner();
                if (lifestealOwner != null)
                {
                    var stats = lifestealOwner.GetComponent<EnemyStats>();
                    if (stats != null)
                    {
                        int healAmount = Mathf.RoundToInt(stats.GetMaxHp() * 0.2f);
                        stats.HealByAmount(healAmount);
                        Debug.Log($"[���� �̳�] {lifestealOwner.name}��(��) {healAmount} ��ŭ ȸ���߽��ϴ�.");
                    }
                    gs.ClearLifestealMinoOwner();
                }

                gs.ClearOccupied();
            }

            linesCompleted++;
        }

        return linesCompleted;
    }

    private void DealDamageToSelectedEnemy(int completedLines)
    {

		if (CharacterManager.selectedCharacter.characterData != null)
        {
			DealDamageToSelectedEnemyInner(completedLines);
		}

    }
	private void DealDamageToSelectedEnemyInner(int completedLines)
    {
        place.Stop();
        del.Play();
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

        int baseDamage = completedLines * columns;
        baseDamage = (int)(baseDamage * ultimateDamageMultiplier);
        baseDamage += additionalExecutionDamage;

        ultimateDamageMultiplier = 1f;
        additionalExecutionDamage = 0;

		AttackEffectSpawner attackEffectSpawner = 
			CharacterManager.selectedCharacter.characterData.AttackEffectSpawner;
        if (attackEffectSpawner != null)
		{
			attackEffectSpawner.Spawn(
				() => {
					enemyStats.ReceiveDamage(baseDamage, columns);
                    StatisticsManager.Instance.TotalDamageDealtThisRun += baseDamage;
                    StatisticsManager.Instance.ClearLineCountThisRun += completedLines;
                    Debug.Log($"���� ������: {baseDamage} (Ŭ���� ��: {completedLines})");
					CheckIfGameEnded();
				}
			);
		}
		else 
		{
			enemyStats.ReceiveDamage(baseDamage, columns);
            StatisticsManager.Instance.TotalDamageDealtThisRun += baseDamage;
            StatisticsManager.Instance.ClearLineCountThisRun += completedLines;
            Debug.Log($"���� ������: {baseDamage} (Ŭ���� ��: {completedLines})");
		}
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
            Debug.Log("��� ���� óġ�Ǿ����ϴ�. ���� ���������� �̵��մϴ�.");

            var treasureEffect = GameObject.FindFirstObjectByType<TreasureEffect>();
            if (treasureEffect != null && treasureEffect.EmergencyFood)
            {
                CharacterManager.instance.RecoverHp(6);
                Debug.Log("[EmergencyFood] ���� ȿ���� ĳ���� HP 6 ȸ����!");
            }

            FindFirstObjectByType<EnemySpawner>().IncreaseDifficulty();
            MoveNextScene();
        }
    }
        
    public void MoveNextScene()
    {
        Debug.Log("���� ������ �̵��մϴ�.");
        StatisticsManager.Instance.CurrentRoom++;
        StatisticsManager.Instance.HighestFloorReached++;
        if (rewardsScreen != null)
            rewardsScreen.SetActive(true);
        else
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

        HoldShape holdShape = FindFirstObjectByType<HoldShape>();
        if (holdShape != null)
        {
            holdShape.RestoreAssignedObject();
            holdShape.UnlockShape();
        }
        else
        {
            Debug.LogWarning("[ResetGrid] HoldShape �ν��Ͻ��� ã�� �� �����ϴ�.");
        }

        foreach (var enemy in enemies)
        {
            var enemyStats = enemy.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.DeactivateDamageMultiplier();
                enemyStats.ResetThorn();
                enemyStats.TickHealingReduction();

            }
        }
        
        StartCoroutine(EnemyTurnSequence());

        TreasureEffect treasureEffect = Object.FindFirstObjectByType<TreasureEffect>();
        if (treasureEffect != null && treasureEffect.CorruptTouch)
        {
            GameObject selectedEnemy = GetSelectedEnemy();
            if (selectedEnemy != null)
            {
                EnemyStats enemyStats = selectedEnemy.GetComponent<EnemyStats>();
                if (enemyStats != null && enemyStats.GetCurrentHp() > 0)
                {
                    enemyStats.ReceiveDamage(1, columns);
                    EffectManager.Instance.OnPoison(selectedEnemy);
                    Debug.Log("[CorruptTouch] ���õ� ���� HP�� 1 ���ҵ�");
                }
            }
        }

        if (treasureEffect != null && treasureEffect.TalismanOfPower)
        {
            turnCounter++;

            if (turnCounter % 2 == 0)
            {
                CharacterManager.selectedCharacter.characterData.CurrentCharacterATK += 1;
                Debug.Log("[TalismanOfPower] 2�� ���! ĳ���� ATK +2 ����");
            }
        }

        Debug.Log("�׸��尡 ���µǾ����ϴ�.");
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
            Debug.Log("�÷��̾� ��� �ϳ��� ���ŵǾ����ϴ�.");
        }
    }

    public void SpawnRandomBlock()
    {
        if (_gridSquares.Count == 0)
        {
            Debug.LogWarning("�׸��忡 ����� ������ ������ �����ϴ�.");
            return;
        }
        List<GridSquare> emptySquares = _gridSquares
            .Select(sq => sq.GetComponent<GridSquare>())
            .Where(sq => !sq.SquareOccupied)
            .ToList();
        if (emptySquares.Count == 0)
        {
            Debug.LogWarning("��� ĭ�� �� �־ ����� ������ ������ �����ϴ�.");
            return;
        }
        int randomIndex = Random.Range(0, emptySquares.Count);
        GridSquare gs = emptySquares[randomIndex];
        gs.SetOccupied();
        gs.ActivateSquare();
        gs.SetBlockSpriteToDefault();
        Debug.Log($"����� ������ ��ġ({gs.SquareIndex})�� DefaultSprite�� �����Ǿ����ϴ�.");
        CheckIfAnyLineIsCompleted();
    }

    public void SelectEnemy(GameObject enemy)
    {

		AttackEffectSpawner attackEffectSpawner =
			CharacterManager.selectedCharacter.characterData.AttackEffectSpawner;

		if (attackEffectSpawner != null) {
			attackEffectSpawner.TargetTransform = enemy.transform;
		}
        selectedEnemy = enemy;
        Debug.Log($"[{selectedEnemy.name}]��(��) �����߽��ϴ�.");
    }

    public void DeactivateRandom4x4()
    {
        if (_gridSquares.Count == 0)
        {
            Debug.LogWarning("�׸��尡 ��� �־� ����� ������ �� �����ϴ�.");
            return;
        }
        int gridSize = 8;
        int x = Random.Range(0, gridSize - 3);
        int y = Random.Range(0, gridSize - 3);
        Debug.Log($"[{x}, {y}] ��ġ���� 4x4 ����� ��Ȱ��ȭ�մϴ�.");

        for (int i = x; i < x + 4; i++)
        {
            for (int j = y; j < y + 4; j++)
            {
                int index = j * gridSize + i;
                if (index < _gridSquares.Count) {
                    _gridSquares[index].GetComponent<GridSquare>().DeactivateBlock();
					MinoEffectHelper.Instance.PlayMinoEffectSingle(_gridSquares[index]);
				}
					
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Destroy(enemy);
            Debug.Log($"[{enemy.name}]��(��) ����Ʈ���� ���ŵǾ����ϴ�.");
        }
        else
        {
            Debug.LogWarning($"[{enemy.name}]��(��) ����Ʈ���� ã�� �� �����ϴ�.");
        }
        CheckIfGameEnded();
    }

    public GameObject GetSelectedEnemy()
    {
        return selectedEnemy;
    }

    public void DropAllBlocks()
    {
        for (int col = 0; col < columns; col++)
        {
            List<GridSquare> occupiedSquares = new List<GridSquare>();
            for (int row = 0; row < rows; row++)
            {
                int index = row * columns + col;
                GridSquare gs = _gridSquares[index].GetComponent<GridSquare>();
                if (gs.SquareOccupied)
                {
                    occupiedSquares.Add(gs);
                }
            }

            for (int row = 0; row < rows; row++)
            {
                int index = row * columns + col;
                GridSquare gs = _gridSquares[index].GetComponent<GridSquare>();
                gs.ClearOccupied();
                gs.Deactivate();
            }

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
        Debug.Log("DropAllBlocks: ��� ����� �Ʒ��� ���������ϴ�.");
    }
    public void Spawn3x3PetrifiedBlocks()
    {
        if (_gridSquares.Count == 0)
        {
            Debug.LogWarning("�׸��尡 ��� �־� ��ȭ ����� ������ �� �����ϴ�.");
            return;
        }

        int safeMin = 1;
        int safeMaxX = columns - 2;
        int safeMaxY = rows - 2;

        int centerX = Random.Range(safeMin, safeMaxX + 1);
        int centerY = Random.Range(safeMin, safeMaxY + 1);

        Debug.Log($"[��ȭ] 3x3 ��� ��ȭ �߽� ��ǥ: ({centerX}, {centerY})");

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                int x = centerX + dx;
                int y = centerY + dy;
                int index = y * columns + x;

                if (index >= 0 && index < _gridSquares.Count)
                {
                    GridSquare gs = _gridSquares[index].GetComponent<GridSquare>();
                    if (!gs.SquareOccupied)
                    {
                        gs.PetrifyBlock();
                    }
                }
            }
        }

    }

    public void SpawnSpecialMino(GameObject enemy)
    {
        if (_gridSquares.Count == 0)
        {
            Debug.LogWarning("�׸��忡 ����� ������ ������ �����ϴ�.");
            return;
        }

        List<GridSquare> emptySquares = _gridSquares
            .Select(sq => sq.GetComponent<GridSquare>())
            .Where(sq => !sq.SquareOccupied)
            .ToList();

        if (emptySquares.Count == 0)
        {
            Debug.LogWarning("��� ĭ�� �� �־ ����� ������ ������ �����ϴ�.");
            return;
        }

        int randomIndex = Random.Range(0, emptySquares.Count);
        GridSquare gs = emptySquares[randomIndex];

        gs.SetOccupied();
        gs.ActivateSquare();
        gs.SetBlockSpriteToDefault();
        gs.SetSpecialMinoOwner(enemy);
        CheckIfAnyLineIsCompleted();

        Debug.Log($"Ư�� ����� ��ġ {gs.SquareIndex}�� �����Ǿ����ϴ�. ������: {enemy.name}");
    }

    public void SpawnHealingMino(GameObject enemy)
    {
        if (_gridSquares.Count == 0) return;

        List<GridSquare> emptySquares = _gridSquares
            .Select(sq => sq.GetComponent<GridSquare>())
            .Where(sq => !sq.SquareOccupied)
            .ToList();

        if (emptySquares.Count == 0) return;

        int randomIndex = Random.Range(0, emptySquares.Count);
        GridSquare gs = emptySquares[randomIndex];

        gs.SetOccupied();
        gs.ActivateSquare();
        gs.SetBlockSpriteToDefault();
        gs.SetLifestealMinoOwner(enemy);
        CheckIfAnyLineIsCompleted();

        Debug.Log($"�� �̳밡 ��ġ {gs.SquareIndex}�� �����Ǿ����ϴ�. ������: {enemy.name}");
    }

    private IEnumerator EnemyTurnSequence()
    {
        yield return new WaitForSeconds(1.5f);

        GameEvents.RequestNewShapes();


        enemies = enemies.Where(enemy => enemy != null && enemy.GetComponent<EnemyStats>() != null).ToList();
        foreach (var enemy in enemies)
        {
            var enemyStats = enemy.GetComponent<EnemyStats>();
            var treasureEffect = Object.FindFirstObjectByType<TreasureEffect>();

            if (enemyStats != null && enemyStats.GetCurrentHp() > 0)
            {

                if (enemy == GetSelectedEnemy())
                {
                    enemyStats.ApplyPoisonFromPlayerDamage();
                }
                
                if (enemyStats.GetPoisonDuration() > 0)
                {
                    enemyStats.ApplyPoisonDamageToPlayer();
                }

                enemyStats.PerformTurnAction(this);
                Debug.Log($"[{enemy.name}]��(��) �÷��̾ �����߽��ϴ�.");

                if (treasureEffect != null && treasureEffect.WoodPile)
                {
                    if (Random.Range(0f, 1f) < 0.5f)
                    {
                        enemyStats.ApplyHealingReduction(1);
                        Debug.Log($"[Woodpile] {enemy.name}���� ġ�� ���� ���� 1 �����!");
                    }
                }
            }
            
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log("[�� ��ȯ] �÷��̾� �� ����!");
    }
}
