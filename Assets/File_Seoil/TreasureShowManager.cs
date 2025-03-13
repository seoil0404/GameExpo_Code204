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
                    currentDescription.DescriptionText = "����\r\n���̻簡 ó������ �ڽ��� ������ �����ߴ� �繰�� ������ ���� ������ �����ʾ� ����� �ڽ��� �������� ���鿴��.\r\n\r\n�ɷ�\r\n���� ���� �׽� [����]���� ��� [����]���� �������� [����]���� ������ �����Ѵ�.";
                    break;
                case CombatData.TreasureType.BusinessAcumen:
                    currentImage.sprite = businessAcumen;
                    currentDescription.DescriptionText = "����\r\n���� ����� Ÿ�� ���ϱ�? , �װ� �ڶ�� ȯ���� �׸� �̷��� ���� ���ϱ�?\r\n\r\n�ɷ�\r\n�� 30���� ȹ���Ҷ� �� 1 ȸ���Ѵ�.";
                    break;
                case CombatData.TreasureType.Condemnation:
                    currentImage.sprite = condemnation;
                    currentDescription.DescriptionText = "����\r\n���� ���� �����Ű�� ���� ���� �ҵ�� ���Ӿ��� ���̸� �����Ѵ�.\r\n\r\n�ɷ�\r\n+ ó���� 5%\r\nó������ ���� ��� ���� ó���մϴ�. (���ε� ó���մϴ�.)\r\nHP �������� ó�� ���� HP�� ǥ�õ˴ϴ�.";
                    break;
                case CombatData.TreasureType.MoneyBack:
                    currentImage.sprite = moneyBack;
                    currentDescription.DescriptionText = "����\r\n������ ����Ʈ�� �� �ָӴ��Դϴ�.\r\n\r\n�ɷ�\r\n�� ���� ȹ��� ��� �� 100�߰�";
                    break;
                case CombatData.TreasureType.EmergencyFood:
                    currentImage.sprite = emergencyFood;
                    currentDescription.DescriptionText = "����\r\n����鿡�� �Ͽ��� �δ��� �����ְ� �ణ�� ��⸦ �ذ��ϴ� �����Դϴ�.\r\n\r\n�ɷ�\r\n�������� Ŭ����� ü�� 6 ȸ��";
                    break;
                case CombatData.TreasureType.GiantResistanceHammer:
                    currentImage.sprite = giantResistanceHammer;
                    currentDescription.DescriptionText = "����\r\n�Ѷ� ���忡�� ���� Ȱ���� �߽��ϴ�.\r\n\r\n�ɷ�\r\nƯ�� ������ ���̵� ����";
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
