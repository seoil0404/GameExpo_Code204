using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class TreasureShowManager : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private CombatData combatData;

    [Header("MonoBehavior")]
    [SerializeField] private Canvas canvas;

    [Header("Prefab")]
    [SerializeField] private GameObject treasurePrefab;

    [Header("Position Setting")]
    [SerializeField] private Vector2 defaultPosition;
    [SerializeField] private float treasureInterval;

    [Header("Sprite Setting")]
    [SerializeField] private Sprite universalGravition;
    [SerializeField] private Sprite businessAcumen;
    [SerializeField] private Sprite condemnation;

    private List<Image> currentTreasureImages;

    private void Awake()
    {
        currentTreasureImages = new List<Image>();
    }

    public void UpdateTreasureImages()
    {
        foreach(Image item in currentTreasureImages)
        {
            Destroy(item.gameObject);
        }

        currentTreasureImages.Clear();

        int index = 0;

        foreach (CombatData.TreasureType item in combatData.TreasureData)
        {
            Image currentImage = Instantiate(treasurePrefab, canvas.transform).GetComponent<Image>();

            currentImage.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultPosition.x + index * treasureInterval, defaultPosition.y);

            switch (item)
            {
                case CombatData.TreasureType.UniversalGravitation:
                    currentImage.sprite = universalGravition;
                    break;
                case CombatData.TreasureType.BusinessAcumen:
                    currentImage.sprite = businessAcumen;
                    break;
                case CombatData.TreasureType.Condemnation:
                    currentImage.sprite = condemnation;
                    break;
                default:
                    Debug.LogError("Unknown TrasureType");
                    currentImage.sprite = null;
                    break;
            }

            index++;
        }
    }
}
