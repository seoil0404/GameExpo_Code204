using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
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

    private void Awake()
    {
        allEnemies.Add(this);
    }

    private void OnDestroy()
    {
		foreach (var block in blocks) block.transform.DOKill();
        allEnemies.Remove(this);
    }

    private void Start()
    {
        blocks = new GameObject[] { topLeftBlock, topRightBlock, bottomLeftBlock, bottomRightBlock };

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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var enemy in allEnemies)
        {
            enemy.Deselect();
        }

        Select();
    }

    private void Select()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
			if (blocks[i] == null) continue;
            blocks[i].SetActive(true);
            blocks[i].transform.DOKill();
            blocks[i].transform.localPosition = originalPositions[i];
        }

        AnimateBlocks();


        Grid grid = FindAnyObjectByType<Grid>();//FindObjectOfType<Grid>();
        if (grid != null)
        {
            grid.SelectEnemy(gameObject);
        }
    }

    private void Deselect()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
			if (blocks[i] == null) continue;
            blocks[i].SetActive(false);
            blocks[i].transform.DOKill();
			blocks[i].transform.localPosition = originalPositions[i];
        }
    }

    private void AnimateBlocks()
    {
        float animationDuration = 0.7f;
        float targetDistance = 10f;

        for (int i = 0; i < blocks.Length; i++)
        {
			if (blocks[i] == null) continue;
            Vector3 expandedPosition = originalPositions[i] + (directions[i].normalized * targetDistance);

            blocks[i].transform.DOLocalMove(expandedPosition, animationDuration)
                .SetLoops(int.MaxValue, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

        }
    }
}
