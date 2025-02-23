using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemView : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private bool isSellTreasure;
    [SerializeField] private CombatData.TreasureType treasureType;

    [SerializeField] private int cost;

    [SerializeField] private GoldData goldData;
    [SerializeField] private CombatData combatData;

    [SerializeField] private TreasureView treasureView;

    private void Awake()
    {
        if(isSellTreasure) switch (treasureType)
        {
            case CombatData.TreasureType.UniversalGravitation:
                treasureView.DescriptionText = "����\r\n���̻簡 ó������ �ڽ��� ������ �����ߴ� �繰�� ������ ���� ������ �����ʾ� ����� �ڽ��� �������� ���鿴��.\r\n\r\n�ɷ�\r\n���� ���� �׽� [����]���� ��� [����]���� �������� [����]���� ������ �����Ѵ�.";
                break;
            case CombatData.TreasureType.BusinessAcumen:
                treasureView.DescriptionText = "����\r\n���� ����� Ÿ�� ���ϱ�? , �װ� �ڶ�� ȯ���� �׸� �̷��� ���� ���ϱ�?\r\n\r\n�ɷ�\r\n�� 30���� ȹ���Ҷ� �� 1 ȸ���Ѵ�.";
                break;
            case CombatData.TreasureType.Condemnation:
                treasureView.DescriptionText = "����\r\n���� ���� �����Ű�� ���� ���� �ҵ�� ���Ӿ��� ���̸� �����Ѵ�.\r\n\r\n�ɷ�\r\n+ ó���� 5%\r\nó������ ���� ��� ���� ó���մϴ�. (���ε� ó���մϴ�.)\r\nHP �������� ó�� ���� HP�� ǥ�õ˴ϴ�.";
                break;
            case CombatData.TreasureType.MoneyBack:
                treasureView.DescriptionText = "����\r\n������ ����Ʈ�� �� �ָӴ��Դϴ�.\r\n\r\n�ɷ�\r\n�� ���� ȹ��� ��� �� 100�߰�";
                break;
            case CombatData.TreasureType.EmergencyFood:
                treasureView.DescriptionText = "����\r\n����鿡�� �Ͽ��� �δ��� �����ְ� �ణ�� ��⸦ �ذ��ϴ� �����Դϴ�.\r\n\r\n�ɷ�\r\n�������� Ŭ����� ü�� 6 ȸ��";
                break;
            case CombatData.TreasureType.GiantResistanceHammer:
                treasureView.DescriptionText = "����\r\n�Ѷ� ���忡�� ���� Ȱ���� �߽��ϴ�.\r\n\r\n�ɷ�\r\nƯ�� ������ ���̵� ����";
                break;
            default:
                break;
        }
        else
        {
            treasureView.DescriptionText = "����\r\n��ü�� �� �� ���� ������ �������Դϴ�.";
        }
    }

    public void OnBuyTreasure()
    {
        if (cost <= goldData.InGameGold)
        {
            goldData.InGameGold -= cost;
            combatData.AddTreasureData(treasureType);
        }
    }

    public void OnBuyHeal()
    {
        if (cost <= goldData.InGameGold)
        {
            goldData.InGameGold -= cost;
            CharacterManager.selectedCharacter.characterData.CurrentHp += 15;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.SetAsLastSibling();
    }
}
