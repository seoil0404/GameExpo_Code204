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
    [SerializeField] private Sprite moneyBack;
    [SerializeField] private Sprite emergencyFood;
    [SerializeField] private Sprite giantResistanceHammer;

    private List<Image> currentTreasureImages;

    public void Initialize()
    {
        Scene.treasureShowManager = this;

        currentTreasureImages = new List<Image>();

        UpdateTreasureImages();
    }

    public void Hide()
    {
        if(currentTreasureImages != null) foreach (Image image in currentTreasureImages) image.gameObject.SetActive(false);
    }

    public void Show()
    {
        if (currentTreasureImages != null) foreach (Image image in currentTreasureImages) image.gameObject.SetActive(true);
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

            TreasureView currentDescription = currentImage.gameObject.GetComponent<TreasureView>();

            currentTreasureImages.Add(currentImage);

            switch (item)
            {
                case CombatData.TreasureType.UniversalGravitation:
                    currentImage.sprite = universalGravition;
                    currentDescription.DescriptionText = "설명\r\n아이사가 처음으로 자신의 마법을 적용했던 사물로 섬세한 마력 조절이 쉽지않아 사과를 자신의 마력으로 물들였다.\r\n\r\n능력\r\n라운드 동안 항시 [공중]적이 모두 [지상]으로 내려오고 [지상]적의 성격을 공유한다.";
                    break;
                case CombatData.TreasureType.BusinessAcumen:
                    currentImage.sprite = businessAcumen;
                    currentDescription.DescriptionText = "설명\r\n그의 재능은 타고난 것일까? , 그가 자라온 환경이 그를 이렇게 만든 것일까?\r\n\r\n능력\r\n돈 30원을 획득할때 피 1 회복한다.";
                    break;
                case CombatData.TreasureType.Condemnation:
                    currentImage.sprite = condemnation;
                    currentDescription.DescriptionText = "설명\r\n주인 또한 집어삼키는 검인 에고 소드는 끊임없이 먹이를 갈망한다.\r\n\r\n능력\r\n+ 처형률 5%\r\n처형률에 따라 모든 것을 처형합니다. (본인도 처형합니다.)\r\nHP 게이지에 처형 가능 HP가 표시됩니다.";
                    break;
                case CombatData.TreasureType.MoneyBack:
                    currentImage.sprite = moneyBack;
                    currentDescription.DescriptionText = "설명\r\n누군가 떨어트린 돈 주머니입니다.\r\n\r\n능력\r\n이 유물 획득시 즉시 돈 100추가";
                    break;
                case CombatData.TreasureType.EmergencyFood:
                    currentImage.sprite = emergencyFood;
                    currentDescription.DescriptionText = "설명\r\n사람들에게 하여금 부담을 덜어주고 약간의 허기를 잊게하는 존재입니다.\r\n\r\n능력\r\n스테이지 클리어시 체력 6 회복";
                    break;
                case CombatData.TreasureType.GiantResistanceHammer:
                    currentImage.sprite = giantResistanceHammer;
                    currentDescription.DescriptionText = "설명\r\n한때 전장에서 많은 활약을 했습니다.\r\n\r\n능력\r\n특수 전투방 난이도 절반";
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
