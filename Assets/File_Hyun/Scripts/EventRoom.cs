using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventRoom : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;

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
    private List<string> eventPool = new() { "길가", "굴", "마녀", "보물상자", "딜레마" };
    private static bool hasDilemmaOccurred = false;

    private void Start()
    {
        eventPool.Remove("딜레마"); //
        if(characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp <= 1) eventPool.Remove("굴");
        ChoiceButtons.SetActive(true);
        EscObject.SetActive(false);
        TriggerRandomEvent();
    }

    private void TriggerRandomEvent()
    {
        if (hasDilemmaOccurred)
        {
            eventPool.Remove("딜레마");
        }

        string randomEvent = eventPool[Random.Range(0, eventPool.Count)];

        if (randomEvent == "딜레마")
        {
            hasDilemmaOccurred = true;
            eventPool.Remove("딜레마");
        }

        Debug.Log($"[이벤트 발생] {randomEvent} 이벤트 시작!");
        StartEvent(randomEvent);
    }

    public void StartEvent(string eventType)
    {
        currentEvent = eventType;

        switch (eventType)
        {
            case "길가":
                SetEvent("탐사를 위해 길을 걷던중 빠르게 어떤 이와 스쳐 지나갑니다.\n순간 당신은 한가지 사실을 깨닫습니다.",
                    "뒤돌아본다", "떠나기");
                break;

            case "굴":
                SetEvent("당신은 땅에서 사람 한명이 간신히 들어갈만한 수상한 구멍을 발견하고 잊고 있던 한가지 사실을 떠올립니다.\n얼마전 보물찾기 축제가 열렸고 구멍에 보물을 숨기는 방식으로 진행된다는 것입니다.\r\n\r\n아마 이 수상한 구멍은 사람들에게 오랜시간 발견되지 않았던것 같습니다.",
                    "사다리를 타고 내려간다", "떠나기");
                break;

            case "마녀":
                SetEvent("갑작스럽게 ‘스어피모’라는 이름을 가진 마녀와 조우합니다.\n마녀는 당신에게 \"무료\"로 포션을 주겠다고 합니다",
                    "받는다", "떠나기");
                break;

            case "보물상자":
                SetEvent("특이하게 생긴 보물상자를 발견했습니다.",
                    "보물상자를 연다", "떠나기");
                break;

            case "딜레마":
                SetEvent("오래된 무덤 앞에 서있습니다.\n무덤 비석에는 '강한 자만이 이 힘을 얻을 수 있으리라'라고 적혀있습니다.",
                    "무덤을 연다", "떠나기");
                break;

            default:
                Debug.LogWarning("잘못된 이벤트 타입");
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
            case "길가":
                if (Random.value < 0.5f)
                {
                    eventText.text = "당신이 잘 알던 모험가가 당신에게 황금 사과를 건넵니다.";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp += 5;
                    Debug.Log("최대 체력 5 증가");
                    RemedyEnd();
                }
                else
                {
                    eventText.text = "뒤돌아 보았을때는 이미 소매치기가 저멀리 당신의 돈을 들고 도망가고 있었습니다.";
                    Debug.Log("돈 50 감소");
                    RemedyEnd();
                }
                break;

            case "굴":
                if (Random.value < 0.7f && characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp >= 1)
                {
                    Debug.Log("체력 1 감소");
                    characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp -= 1;
                    SetEvent("아직 바닥이 보이지 않습니다.",
                    "계속 내려간다", "떠나기");
                }
                else
                {
                    goldData.InGameGold += 100;
                    Debug.Log("돈 100 획득");
                    eventText.text = "오랫동안 잊혀진, 묻혀진, 숨겨진 보물을 찾았습니다.";
                    RemedyEnd();
                }
                break;

            case "마녀":
                if (Random.value < 0.5f)
                {
                    eventText.text = "마녀는 붉은 포션을 주었습니다.\n포션은 쓰고 떫었습니다. 당신은 고양감을 느낍니다";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp -= 7;
                    characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp += 7;
                    Debug.Log("체력 7 감소, 최대 체력 7 증가");
                    RemedyEnd();
                }
                else
                {
                    eventText.text = "마녀는 푸른 포션을 주었습니다.\n포션은 포션은 달고 상쾌했습니다.";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp += 13;
                    Debug.Log("체력 13 증가");
                    RemedyEnd();
                }
                break;

            case "보물상자":
                if (Random.value < 0.8f)
                {
                    eventText.text = "당신은 체력 포션을 찾았습니다.";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp += 15;
                    Debug.Log("체력 15 증가");
                    RemedyEnd();
                }
                else
                {
                    eventText.text = "미믹이 당신을 가볍게 물고 빠르게 도망칩니다.";
                    characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp -= 5;
                    Debug.Log("체력 5 감소");
                    RemedyEnd();
                }
                break;

            case "딜레마":
                eventText.text = "당신은 강력한 힘이 깃든 유물을 얻었지만, 그 대가로 영원히 생명력의 일부를 잃었습니다.";
                characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp -= 10;

                Debug.Log("최대 체력 10 감소, 증폭하는 힘의 탈리스만 획득");
                RemedyEnd();
                break;
        }
    }

    private void Option2Selected()
    {
        eventText.text = "당신은 무시하고 다시 걷기 시작합니다.";
        Debug.Log("떠나기 선택 - 아무 일도 일어나지 않음");
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
        Debug.Log("이벤트방 종료");
    }
}