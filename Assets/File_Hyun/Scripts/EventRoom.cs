using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static CombatData;

public class EventRoom : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;
    public CombatData combatData;

    public GoldData goldData;
    public GameObject eventUI;
    public GameObject ChoiceButtons;
    public GameObject EscObject;
    public Text eventText;
    public Text option1Text;
    public Text option2Text;
    public Button option1Button;
    public Button option2Button;
    public Button EscButton;

    private string currentEvent;
    private List<string> eventPool = new() { "�氡", "��", "����", "��������", "������" };

    private void Start()
    {
        if (characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp <= 1) eventPool.Remove("��");
        if (combatData.TreasureData.Contains(TreasureType.TalismanOfPower)) eventPool.Remove("������");
        ChoiceButtons.SetActive(true);
        EscObject.SetActive(false);

        string randomEvent = eventPool[Random.Range(0, eventPool.Count)];
        Debug.Log($"[�̺�Ʈ �߻�] {randomEvent} �̺�Ʈ ����!");
        StartEvent(randomEvent);
    }

    public void StartEvent(string eventType)
    {
        currentEvent = eventType;

        switch (eventType)
        {
            case "�氡":
                SetEvent("Ž�縦 ���� ���� �ȴ��� ������ � �̿� ���� �������ϴ�.\n���� ����� �Ѱ��� ����� ���ݽ��ϴ�.",
                    "�ڵ��ƺ���", "������");
                break;

            case "��":
                SetEvent("����� ������ ��� �Ѹ��� ������ ������ ������ ������ �߰��ϰ� �ذ� �ִ� �Ѱ��� ����� ���ø��ϴ�.\n���� ����ã�� ������ ���Ȱ� ���ۿ� ������ ����� ������� ����ȴٴ� ���Դϴ�.\r\n\r\n�Ƹ� �� ������ ������ ����鿡�� �����ð� �߰ߵ��� �ʾҴ��� �����ϴ�.",
                    "��ٸ��� Ÿ�� ��������", "������");
                break;

            case "����":
                SetEvent("���۽����� �������Ǹ𡯶�� �̸��� ���� ����� �����մϴ�.\n����� ��ſ��� \"����\"�� ������ �ְڴٰ� �մϴ�",
                    "�޴´�", "������");
                break;

            case "��������":
                SetEvent("Ư���ϰ� ���� �������ڸ� �߰��߽��ϴ�.",
                    "�������ڸ� ����", "������");
                break;

            case "������":
                SetEvent("������ ���� �տ� ���ֽ��ϴ�.\n���� �񼮿��� '���� �ڸ��� �� ���� ���� �� ��������'��� �����ֽ��ϴ�.",
                    "������ ����", "������");
                break;

            default:
                Debug.LogWarning("�߸��� �̺�Ʈ Ÿ��");
                return;
        }
    }

    private void SetEvent(string description, string option1, string option2)
    {
        eventText.text = description;
        option1Text.text = option1;
        option2Text.text = option2;

        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();

        option1Button.onClick.AddListener(Option1Selected);
        option2Button.onClick.AddListener(Option2Selected);
    }

    private void Option1Selected()
    {
        switch (currentEvent)
        {
            case "�氡":
                if (Random.value < 0.5f)
                {
                    eventText.text = "����� �� �˴� ���谡�� ��ſ��� Ȳ�� ����� �ǳܴϴ�.\n\n�ִ�ü�� +5";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp += 5;
                    Debug.Log("�ִ� ü�� 5 ����");
                    RemedyEnd();
                }
                else
                {
                    eventText.text = "�ڵ��� ���������� �̹� �Ҹ�ġ�Ⱑ ���ָ� ����� ���� ��� �������� �־����ϴ�.\n\n-50G";
                    Debug.Log("�� 50 ����");
                    RemedyEnd();
                }
                break;

            case "��":
                if (Random.value < 0.7f && characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp >= 1)
                {
                    Debug.Log("ü�� 1 ����");
                    characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp -= 1;
                    SetEvent("���� �ٴ��� ������ �ʽ��ϴ�.\n\nü�� -1",
                    "��� ��������", "������");
                }
                else
                {
                    goldData.InGameGold += 100;
                    Debug.Log("�� 100 ȹ��");
                    eventText.text = "�������� ������, ������, ������ ������ ã�ҽ��ϴ�.\n\n+100G";
                    RemedyEnd();
                }
                break;

            case "����":
                if (Random.value < 0.5f)
                {
                    eventText.text = "����� ���� ������ �־����ϴ�.\n������ ���� �������ϴ�. ����� ��簨�� �����ϴ�\n\nü�� -7\n�ִ�ü�� +7";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp -= 7;
                    characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp += 7;
                    Debug.Log("ü�� 7 ����, �ִ� ü�� 7 ����");
                    RemedyEnd();
                }
                else
                {
                    eventText.text = "����� Ǫ�� ������ �־����ϴ�.\n������ ������ �ް� �����߽��ϴ�.\n\nü�� +13";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp += 13;
                    Debug.Log("ü�� 13 ����");
                    RemedyEnd();
                }
                break;

            case "��������":
                if (Random.value < 0.8f)
                {
                    eventText.text = "����� ü�� ������ ã�ҽ��ϴ�.\n\nü�� +15";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp += 15;
                    Debug.Log("ü�� 15 ����");
                    RemedyEnd();
                }
                else
                {
                    eventText.text = "�̹��� ����� ������ ���� ������ ����Ĩ�ϴ�.\n\nü�� -5";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp -= 5;
                    Debug.Log("ü�� 5 ����");
                    RemedyEnd();
                }
                break;

            case "������":
                eventText.text = "����� ������ ���� ��� ������ �������, �� �밡�� ������ ������� �Ϻθ� �Ҿ����ϴ�.\n\n�ִ�ü�� -10\n���� '�����ϴ� ���� Ż������' ȹ��";
                characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp -= 10;
                combatData.AddTreasureData(TreasureType.TalismanOfPower);
                Debug.Log("�ִ� ü�� 10 ����, �����ϴ� ���� Ż������ ȹ��");
                RemedyEnd();
                break;
        }
    }

    private void Option2Selected()
    {
        eventText.text = "����� �����ϰ� �ٽ� �ȱ� �����մϴ�.";
        Debug.Log("������ ���� - �ƹ� �ϵ� �Ͼ�� ����");
        EscButton.onClick.AddListener(Disable);
        RemedyEnd();
    }

    private void Disable()
    {
        eventUI.SetActive(false);
    }

    private void RemedyEnd()
    {
        ChoiceButtons.SetActive(false);
        EscObject.SetActive(true);
        EscButton.onClick.AddListener(EndEventRoom);
    }

    private void EndEventRoom()
    {
        Scene.Controller.OnClearScene();
        StatisticsManager.Instance.CurrentRoom++;
        StatisticsManager.Instance.HighestFloorReached++;
        Debug.Log("�̺�Ʈ�� ����");
    }
}