using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Slider volumeSlider;
    public Text volumeText;

    public Button Koreanbutton;
    public Button Englishbutton;
    public Text LanguageText;

    public string Language = "�ѱ���";

    void Start()
    {
        volumeSlider.value = Mathf.RoundToInt(AudioListener.volume * 100);
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        UpdateVolume(volumeSlider.value);

        Koreanbutton.onClick.AddListener(() => UpdateLanguage("������: �ѱ���"));
        Englishbutton.onClick.AddListener(() => UpdateLanguage("������: ��  ��"));
    }

    void UpdateVolume(float value)
    {
        AudioListener.volume = value / 100f;
        volumeText.text = $"{value}%";
    }

    void UpdateLanguage(string Text)
    {
        LanguageText.text = Text;
        Debug.Log(Text);
        Language = Text.Replace("������: ", "");
    }
}