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
    [SerializeField] private Sprite nobleBlood;
    [SerializeField] private Sprite boneCanine;
    [SerializeField] private Sprite goldAndSilver;
    [SerializeField] private Sprite goldenHair;
    [SerializeField] private Sprite woodPile;
    [SerializeField] private Sprite corruptTouch;
    [SerializeField] private Sprite ringOfTime;
    [SerializeField] private Sprite holyShield;
    [SerializeField] private Sprite etherMemberShip;
    [SerializeField] private Sprite soulLantern;
    [SerializeField] private Sprite totemOfResistance;
    [SerializeField] private Sprite orderDefenceCloak;
    [SerializeField] private Sprite caneOfGravity;
    [SerializeField] private Sprite talismanOfPower;
    [SerializeField] private Sprite swordOfRuinedKing;
    [SerializeField] private Sprite shoesOfHermes;
    [SerializeField] private Sprite goldenApple;
    [SerializeField] private Sprite multipleCureScroll;

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
                case CombatData.TreasureType.NobleBlood:
                    currentImage.sprite = nobleBlood;
                    currentDescription.DescriptionText = "����\r\n���� �������� ���Դϴ�.\r\n\r\n�ɷ�\r\n���� ���� �� ���� ü���� 10% ȸ��";
                    break;
                case CombatData.TreasureType.BoneCanine:
                    currentImage.sprite = boneCanine;
                    currentDescription.DescriptionText = "����\r\n�Ѷ� �ʿ��� ȣ���� ������ �ƶ���� �۰����Դϴ�.\r\n\r\n�ɷ�\r\n������ ����� ��, �߰� ������ +8";
                    break;
                case CombatData.TreasureType.GoldAndSilver:
                    currentImage.sprite = goldAndSilver;
                    currentDescription.DescriptionText = "����\r\n�ݰ� ���� ������ ��ġ �ִ� �ڿ��Դϴ�.\r\n\r\n�ɷ�\r\n���� ��ũ�� ȹ�� �� 8��� ȹ��";
                    break;
                case CombatData.TreasureType.GoldenHair:
                    currentImage.sprite = goldenHair;
                    currentDescription.DescriptionText = "����\r\n���� �Ӹ��� �߶�����, �ݹ��� ���� ��� �߾\r\n\r\n�ɷ�\r\n64 ������ �̻� ���� ��� ���� ü���� ���� ȸ��";
                    break;
                case CombatData.TreasureType.WoodPile:
                    currentImage.sprite = woodPile;
                    currentDescription.DescriptionText = "����\r\nƯ�� �������� �ڶ�� �������� ������ ����Ͽ� ���� ���� �����̾� ���͵��� ���� �������� �����Դϴ�.\r\n\r\n�ɷ�\r\n��밡 ������ �ߵ��� ��, �� 1 ����";
                    break;
                case CombatData.TreasureType.CorruptTouch:
                    currentImage.sprite = corruptTouch;
                    currentDescription.DescriptionText = "����\r\n����� ���� �������ϴ�.\r\n\r\n�ɷ�\r\n��뿡�� ��2 ���¸� 1�� �ο�";
                    break;
                case CombatData.TreasureType.RingOfTime:
                    currentImage.sprite = ringOfTime;
                    currentDescription.DescriptionText = "����\r\n�ð��� �ٷ�� ����� ������ ��� �����Դϴ�.\r\n\r\n�ɷ�\r\n�ñر� �ʿ� Ŭ���� ������ 4/5�� ����";
                    break;
                case CombatData.TreasureType.HolyShield:
                    currentImage.sprite = holyShield;
                    currentDescription.DescriptionText = "����\r\nõ���� ���� ���� �����, �����ڴ� �ż��� ��ȣ�� �ް� �Ǹ�, �������� �Ʊ��� ��ȣ�ϴ� �ɷ��� �����ϴ�.\r\n\r\n�ɷ�\r\n���� ���� ��, ��ȿȭ ���·� ����";
                    break;
                case CombatData.TreasureType.EtherMemberShip:
                    currentImage.sprite = etherMemberShip;
                    currentDescription.DescriptionText = "����\r\n���׸� ��ȸ VIP���� �����մϴ�.\r\n\r\n�ɷ�\r\n���� ��� ǰ�� ���� 20% ����";
                    break;
                case CombatData.TreasureType.SoulLantern:
                    currentImage.sprite = soulLantern;
                    currentDescription.DescriptionText = "����\r\n������ �Ƿ��� �� ���� ������ �����ϰڴٴ� �߽ɿ� ���� ��Ȥ�� ����� ������ �ʽ��ϴ�.\r\n\r\n�ɷ�\r\n���� óġ�� �� �� 1 ����";
                    break;
                case CombatData.TreasureType.TotemOfResistance:
                    currentImage.sprite = totemOfResistance;
                    currentDescription.DescriptionText = "����\r\n������ ������ ����� �������̳� ����� ��Ű�� ��ȣ�ŵ��� ���� �ż��� �����Դϴ�.\r\n\r\n�ɷ�\r\nó�� ���ϴ°��� 1�� ��ȣ�ϰ� ü���� ó��ġ ü�¸�ŭ ȸ��";
                    break;
                case CombatData.TreasureType.OrderDefenceCloak:
                    currentImage.sprite = orderDefenceCloak;
                    currentDescription.DescriptionText = "����\r\n������ ��� �����簡 ������ ���� �ŵα� ����, �ڽ��� ������� ��� ������ ���Դϴ�.\r\n\r\n�ɷ�\r\n3��°�� �����ϴ� ������������ ó�� �޴� �����(�̳���� ����) ���";
                    break;
                case CombatData.TreasureType.CaneOfGravity:
                    currentImage.sprite = caneOfGravity;
                    currentDescription.DescriptionText = "����\r\n�� �����̸� ���� �߷��� ��ȭ��Ű�ų� ������ �� �ֽ��ϴ�.\r\n\r\n�ɷ�\r\n���� ���� �� ������ �� �ϳ��� ħ��";
                    break;
                case CombatData.TreasureType.TalismanOfPower:
                    currentImage.sprite = talismanOfPower;
                    currentDescription.DescriptionText = "����\r\n��뿡 ���� ���õ� �ɷ��� ��� �� �������� ��� �Ǿ����ϴ�.\r\n\r\n�ɷ�\r\n2�ϸ��� �� 1 ����";
                    break;
                case CombatData.TreasureType.SwordOfRuinedKing:
                    currentImage.sprite = swordOfRuinedKing;
                    currentDescription.DescriptionText = "����\r\n�Ѷ� ������ ���� ����ߴ� ���Դϴ�.\r\n\r\n�ɷ�\r\n����� ���� ü���� 8% ��ŭ ���� �������� �߰�";
                    break;
                case CombatData.TreasureType.ShoesOfHermes:
                    currentImage.sprite = shoesOfHermes;
                    currentDescription.DescriptionText = "����\r\n�Ź߿� ������ �޷������� �������� �ʽ��ϴ�.\r\n\r\n�ɷ�\r\nȸ�� Ȯ�� 10% ����";
                    break;
                case CombatData.TreasureType.GoldenApple:
                    currentImage.sprite = goldenApple;
                    currentDescription.DescriptionText = "����\r\nȲ�ݺ� ����Դϴ�.\r\n\r\n�ɷ�\r\n�ִ� ü�� 10 ���";
                    break;
                case CombatData.TreasureType.MultipleCureScroll:
                    currentImage.sprite = multipleCureScroll;
                    currentDescription.DescriptionText = "����\r\n���� ����ڴ� �������� ���� ������� �ο� �� �ְ� �˴ϴ�.\r\n\r\n�ɷ�\r\n���� ��ũ�� ��� �� ü�� 3 ȸ��";
                    break;
                default:
                    Debug.LogError("Unknown TrasureType : " + item.ToString());
                    currentImage.sprite = null;
                    break;
            }

            index++;
        }
    }
}
