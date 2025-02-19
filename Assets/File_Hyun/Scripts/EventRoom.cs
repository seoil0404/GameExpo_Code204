using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EventRoom : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;

    public GameObject eventUI;
    public GameObject ChoiceButtons;
    public GameObject EscObject;
    public Text eventText;
    public Text option1Text;
    public Text option2Text;
    public Button option1Button;
    public Button option2Button;
    public Button EscButton;

    private string currentEvent; // ���� �̺�Ʈ �̸�
    private List<string> eventPool = new List<string> { "�氡", "��", "����", "��������", "������" };
    private static bool hasDilemmaOccurred = false;

    private void Start()
    {
        ChoiceButtons.SetActive(true);
        EscObject.SetActive(false);
        TriggerRandomEvent();
    }

    private void TriggerRandomEvent()
    {
        if (hasDilemmaOccurred)
        {
            eventPool.Remove("������");
        }

        string randomEvent = eventPool[Random.Range(0, eventPool.Count)];

        if (randomEvent == "������")
        {
            hasDilemmaOccurred = true;
            eventPool.Remove("������");
        }

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
                    eventText.text = "����� �� �˴� ���谡�� ��ſ��� Ȳ�� ����� �ǳܴϴ�.";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp += 5;
                    Debug.Log("�ִ� ü�� 5 ����");
                    EventEnd();
                }
                else
                {
                    eventText.text = "�ڵ��� ���������� �̹� �Ҹ�ġ�Ⱑ ���ָ� ����� ���� ��� �������� �־����ϴ�.";
                    Debug.Log("�� 50 ����");
                    EventEnd();
                }
                break;

            case "��":
                if (Random.value < 0.7f)
                {
                    Debug.Log("ü�� 1 ����");
                    SetEvent("���� �ٴ��� ������ �ʽ��ϴ�.",
                    "��� ��������", "������");
                }
                else
                {
                    eventText.text = "�������� ������, ������, ������ ������ ã�ҽ��ϴ�.";
                    Debug.Log("�� 100 ȹ��");
                    EventEnd();
                }
                break;

            case "����":
                if (Random.value < 0.5f)
                {
                    eventText.text = "����� ���� ������ �־����ϴ�.\n������ ���� �������ϴ�. ����� ��簨�� �����ϴ�";
                    Debug.Log("ü�� 7 ����, �ִ� ü�� 7 ����");
                    EventEnd();
                }
                else
                {
                    eventText.text = "����� Ǫ�� ������ �־����ϴ�.\n������ ������ �ް� �����߽��ϴ�.";
                    Debug.Log("ü�� 13 ����");
                    EventEnd();
                }
                break;

            case "��������":
                if (Random.value < 0.8f)
                {
                    eventText.text = "����� ü�� ������ ã�ҽ��ϴ�.";
                    Debug.Log("ü�� 15 ����");
                    EventEnd();
                }
                else
                {
                    eventText.text = "�̹��� ����� ������ ���� ������ ����Ĩ�ϴ�.";
                    Debug.Log("ü�� 5 ����");
                    EventEnd();
                }
                break;

            case "������":
                eventText.text = "����� ������ ���� ��� ������ �������, �� �밡�� ������ ������� �Ϻθ� �Ҿ����ϴ�.";
                Debug.Log("�ִ� ü�� 10 ����, �����ϴ� ���� Ż������ ȹ��");
                EventEnd();
                break;
        }
    }

    private void Option2Selected()
    {
        eventText.text = "����� �����ϰ� �ٽ� �ȱ� �����մϴ�.";
        Debug.Log("������ ���� - �ƹ� �ϵ� �Ͼ�� ����");
        EscButton.onClick.AddListener(Disable);
        EventEnd();
    }

    private void Disable()
    {
        eventUI.SetActive(false);
    }

    private void EventEnd()
    {
        ChoiceButtons.SetActive(false);
        EscObject.SetActive(true);
        EscButton.onClick.AddListener(Disable);
        GameObject.Find("SceneController").GetComponent<SceneController>().OnClearScene();
        Debug.Log("�̺�Ʈ�� ����");
    }
}