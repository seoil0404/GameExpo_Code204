using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class EnemySelector : MonoBehaviour, IPointerClickHandler
{
    public GameObject topLeftBlock;
    public GameObject topRightBlock;
    public GameObject bottomLeftBlock;
    public GameObject bottomRightBlock;

    private GameObject[] blocks;
    private Vector3[] originalPositions;
    private Vector3[] directions;

    private static List<EnemySelector> allEnemies = new List<EnemySelector>();
    private static EnemySelector selectedEnemy = null;
    private static bool enemySelectedAtStart = false;

    private void Awake()
    {
        allEnemies.Add(this);
    }

    private void OnDestroy()
    {
        if (blocks != null)
        {
            foreach (var block in blocks)
            {
                if (block != null)
                {
                    block.transform.DOKill();
                }
            }
        }

        allEnemies.Remove(this);

        if (selectedEnemy == this)
        {
            selectedEnemy = null;

            if (allEnemies.Count > 0)
            {
                Grid grid = FindAnyObjectByType<Grid>();
                if (grid != null)
                {
                    grid.StartCoroutine(DelayedSelectRandomEnemy());
                }
            }
        }
    }



    private void Start()
    {
        blocks = new GameObject[] { topLeftBlock, topRightBlock, bottomLeftBlock, bottomRightBlock };

        if (blocks == null)
            return;

        originalPositions = new Vector3[blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] != null)
            {
                originalPositions[i] = blocks[i].transform.localPosition;
                blocks[i].SetActive(false);
            }
        }

        directions = new Vector3[]
        {
            new Vector3(-1, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0)
        };

        if (!enemySelectedAtStart)
        {
            enemySelectedAtStart = true;
            StartCoroutine(DelayedSelectRandomEnemy());
        }




    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedEnemy == this)
            return;

        foreach (var enemy in allEnemies)
        {
            enemy.Deselect();
        }

        Select();
    }

    private void Select()
    {
        if (selectedEnemy == this)
            return;

        selectedEnemy = this;

        if (blocks == null || blocks.Length == 0)
            return;

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == null)
                continue;

            blocks[i].SetActive(true);
            blocks[i].transform.DOKill();
            blocks[i].transform.localPosition = originalPositions[i];
        }

        AnimateBlocks();

        Grid grid = FindAnyObjectByType<Grid>();
        if (grid != null)
        {
            grid.SelectEnemy(gameObject);
        }

        Canvas.ForceUpdateCanvases();
    }

    private void Deselect()
    {
        if (blocks == null)
            return;

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == null)
                continue;

            blocks[i].SetActive(false);
            blocks[i].transform.DOKill();
            blocks[i].transform.localPosition = originalPositions[i];
        }
    }

    private void AnimateBlocks()
    {
        float animationDuration = 0.7f;
        float targetDistance = 10f;

        if (blocks == null)
            return;

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == null)
                continue;

            Vector3 expandedPosition = originalPositions[i] + (directions[i].normalized * targetDistance);

            blocks[i].transform.DOLocalMove(expandedPosition, animationDuration)
                .SetLoops(int.MaxValue, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }

    public static void SelectRandomEnemy()
    {
        if (allEnemies == null || allEnemies.Count == 0)
        {
            selectedEnemy = null;
            return;
        }

        if (selectedEnemy != null)
            return;

        int randomIndex = Random.Range(0, allEnemies.Count);
        selectedEnemy = allEnemies[randomIndex];

        if (selectedEnemy != null && selectedEnemy.blocks != null && selectedEnemy.blocks.Length > 0)
        {
            foreach (var enemy in allEnemies)
            {
                if (enemy != selectedEnemy)
                    enemy.Deselect();
            }

            selectedEnemy.Select();

            Grid grid = FindAnyObjectByType<Grid>();
            if (grid != null)
            {
                grid.SelectEnemy(selectedEnemy.gameObject);
            }

            for (int i = 0; i < selectedEnemy.blocks.Length; i++)
            {
                if (selectedEnemy.blocks[i] == null)
                    continue;

                selectedEnemy.blocks[i].SetActive(true);
                selectedEnemy.blocks[i].transform.DOKill();
                selectedEnemy.blocks[i].transform.localPosition = selectedEnemy.originalPositions[i];
            }

            selectedEnemy.AnimateBlocks();
            Canvas.ForceUpdateCanvases();
        }
    }

    private IEnumerator DelayedSelectRandomEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        SelectRandomEnemy();
    }
}
