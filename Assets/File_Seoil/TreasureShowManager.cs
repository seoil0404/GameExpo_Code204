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
                case CombatData.TreasureType.NobleBlood:
                    currentImage.sprite = nobleBlood;
                    currentDescription.DescriptionText = "설명\r\n고위 흡혈귀의 피입니다.\r\n\r\n능력\r\n전투 시작 시 현재 체력의 10% 회복";
                    break;
                case CombatData.TreasureType.BoneCanine:
                    currentImage.sprite = boneCanine;
                    currentDescription.DescriptionText = "설명\r\n한때 초원을 호령한 사자인 아라곤의 송곳니입니다.\r\n\r\n능력\r\n보스가 상대일 때, 추가 데미지 +8";
                    break;
                case CombatData.TreasureType.GoldAndSilver:
                    currentImage.sprite = goldAndSilver;
                    currentDescription.DescriptionText = "설명\r\n금과 은은 고대부터 가치 있는 자원입니다.\r\n\r\n능력\r\n마법 스크롤 획득 시 8골드 획득";
                    break;
                case CombatData.TreasureType.GoldenHair:
                    currentImage.sprite = goldenHair;
                    currentDescription.DescriptionText = "설명\r\n“넌 머리를 잘랐지만, 금발의 삶을 살곤 했어”\r\n\r\n능력\r\n64 데미지 이상 입힐 경우 현재 체력을 전부 회복";
                    break;
                case CombatData.TreasureType.WoodPile:
                    currentImage.sprite = woodPile;
                    currentDescription.DescriptionText = "설명\r\n특정 나무에서 자라는 성스러운 나무를 사용하여 만든 대의 뱀파이어 헌터들이 만든 전설적인 무기입니다.\r\n\r\n능력\r\n상대가 흡혈을 발동할 때, 힘 1 증가";
                    break;
                case CombatData.TreasureType.CorruptTouch:
                    currentImage.sprite = corruptTouch;
                    currentDescription.DescriptionText = "설명\r\n어둠의 힘이 깃들었습니다.\r\n\r\n능력\r\n상대에게 독2 상태를 1턴 부여";
                    break;
                case CombatData.TreasureType.RingOfTime:
                    currentImage.sprite = ringOfTime;
                    currentDescription.DescriptionText = "설명\r\n시간을 다루는 고대의 마법이 깃든 반지입니다.\r\n\r\n능력\r\n궁극기 필요 클리어 라인을 4/5로 변경";
                    break;
                case CombatData.TreasureType.HolyShield:
                    currentImage.sprite = holyShield;
                    currentDescription.DescriptionText = "설명\r\n천상의 힘을 담은 팔찌로, 착용자는 신성한 보호를 받게 되며, 전투에서 아군을 보호하는 능력을 가집니다.\r\n\r\n능력\r\n전투 시작 시, 무효화 상태로 변경";
                    break;
                case CombatData.TreasureType.EtherMemberShip:
                    currentImage.sprite = etherMemberShip;
                    currentDescription.DescriptionText = "설명\r\n에테르 상회 VIP에게 발행합니다.\r\n\r\n능력\r\n상점 모든 품목 가격 20% 감소";
                    break;
                case CombatData.TreasureType.SoulLantern:
                    currentImage.sprite = soulLantern;
                    currentDescription.DescriptionText = "설명\r\n군도의 악령은 이 세상에 고통을 전파하겠다는 야심에 넘쳐 잔혹한 사냥을 멈추지 않습니다.\r\n\r\n능력\r\n적을 처치할 시 힘 1 증가";
                    break;
                case CombatData.TreasureType.TotemOfResistance:
                    currentImage.sprite = totemOfResistance;
                    currentDescription.DescriptionText = "설명\r\n저항의 토템은 고대의 정령족이나 대륙을 지키던 수호신들이 만들어낸 신성한 유물입니다.\r\n\r\n능력\r\n처형 당하는것을 1번 보호하고 체력을 처형치 체력만큼 회복";
                    break;
                case CombatData.TreasureType.OrderDefenceCloak:
                    currentImage.sprite = orderDefenceCloak;
                    currentDescription.DescriptionText = "설명\r\n위대한 방어 마법사가 마지막 숨을 거두기 직전, 자신의 생명력을 담아 직조한 것입니다.\r\n\r\n능력\r\n3번째로 입장하는 스테이지마다 처음 받는 디버프(미노방해 제외) 방어";
                    break;
                case CombatData.TreasureType.CaneOfGravity:
                    currentImage.sprite = caneOfGravity;
                    currentDescription.DescriptionText = "설명\r\n이 지팡이를 통해 중력을 변화시키거나 제어할 수 있습니다.\r\n\r\n능력\r\n전투 시작 시 무작위 적 하나를 침묵";
                    break;
                case CombatData.TreasureType.TalismanOfPower:
                    currentImage.sprite = talismanOfPower;
                    currentDescription.DescriptionText = "설명\r\n고대에 힘과 관련된 능력은 모두 이 부적에서 비롯 되었습니다.\r\n\r\n능력\r\n2턴마다 힘 1 증가";
                    break;
                case CombatData.TreasureType.SwordOfRuinedKing:
                    currentImage.sprite = swordOfRuinedKing;
                    currentDescription.DescriptionText = "설명\r\n한때 군도의 왕이 사용했던 검입니다.\r\n\r\n능력\r\n상대의 현재 체력의 8% 만큼 최종 데미지에 추가";
                    break;
                case CombatData.TreasureType.ShoesOfHermes:
                    currentImage.sprite = shoesOfHermes;
                    currentDescription.DescriptionText = "설명\r\n신발에 날개가 달려있으나 움직이진 않습니다.\r\n\r\n능력\r\n회피 확률 10% 증가";
                    break;
                case CombatData.TreasureType.GoldenApple:
                    currentImage.sprite = goldenApple;
                    currentDescription.DescriptionText = "설명\r\n황금빛 사과입니다.\r\n\r\n능력\r\n최대 체력 10 상승";
                    break;
                case CombatData.TreasureType.MultipleCureScroll:
                    currentImage.sprite = multipleCureScroll;
                    currentDescription.DescriptionText = "설명\r\n마법 사용자는 전투에서 더욱 끈질기게 싸울 수 있게 됩니다.\r\n\r\n능력\r\n마법 스크롤 사용 시 체력 3 회복";
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
