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
    private Vector3[] directions;
    private bool isSelected = false;

    private static List<EnemySelector> allEnemies = new List<EnemySelector>();

    private void Awake()
    {
        
        allEnemies.Add(this);
    }

    private void OnDestroy()
    {
      
        allEnemies.Remove(this);
    }

    private void Start()
    {
        blocks = new GameObject[] { topLeftBlock, topRightBlock, bottomLeftBlock, bottomRightBlock };

        directions = new Vector3[]
        {
            new Vector3(-1, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0)
        };

        foreach (var block in blocks)
        {
            if (block != null)
                block.SetActive(false);
        }
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
        isSelected = true;

        foreach (var block in blocks)
        {
            if (block != null)
                block.SetActive(true);
        }

        AnimateBlocks();
    }

    private void Deselect()
    {
        isSelected = false;

        foreach (var block in blocks)
        {
            if (block != null)
            {
                block.SetActive(false);
                block.transform.DOKill();
            }
        }
    }

    private void AnimateBlocks()
    {
        float animationDuration = 0.3f;
        float targetDistance = 10f;
        int loopCount = 3;

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] != null)
            {
                Vector3 originalPosition = blocks[i].transform.localPosition;
                Vector3 expandedPosition = originalPosition + (directions[i].normalized * targetDistance);

                blocks[i].transform.DOLocalMove(expandedPosition, animationDuration)
                    .SetLoops(loopCount * 2, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
            }
        }
    }
}
