using UnityEngine;
using UnityEngine.UI;

public class UltimateGauge : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;

    public Text UltimateText;
    public Image IsaUltimateGauge;
    public Image BlayrinUltimateGauge;
    public Image HughUltimateGauge;
    public Button UseButton;

    void Start()
    {
        IsaUltimateGauge.enabled = false;
        BlayrinUltimateGauge.enabled = false;
        HughUltimateGauge.enabled = false;
        switch(GameData.SelectedCharacterIndex)
        {
            case 1:
                IsaUltimateGauge.enabled = true;
                break;
            case 2:
                BlayrinUltimateGauge.enabled = true;
                break;
            case 3:
                HughUltimateGauge.enabled = true;
                break;
        }
        characters[GameData.SelectedCharacterIndex - 1].characterData.OnUltimateGaugeChanged += UpdateUltimateGauge;
        UpdateUltimateGauge();
    }

    void OnDestroy()
    {
        if (characters != null && characters.Length > 0 && characters[GameData.SelectedCharacterIndex - 1] != null)
        {
            characters[GameData.SelectedCharacterIndex - 1].characterData.OnUltimateGaugeChanged -= UpdateUltimateGauge;
        }
    }

    void UpdateUltimateGauge()
    {
        float current = characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentUltimateGauge;
        float max = characters[GameData.SelectedCharacterIndex - 1].characterData.MaxUltimateGauge;
        IsaUltimateGauge.fillAmount = current / max;
        BlayrinUltimateGauge.fillAmount = current / max;
        HughUltimateGauge.fillAmount = current / max;
        if (characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentUltimateGauge == characters[GameData.SelectedCharacterIndex - 1].characterData.MaxUltimateGauge)
        {
            UseButton.interactable = true;
            UltimateText.text = "MAX!";
        }
        else
        {
            UseButton.interactable = false;
            UltimateText.text = characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentUltimateGauge + " / " + characters[GameData.SelectedCharacterIndex - 1].characterData.MaxUltimateGauge;
        }
    }
}
