using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Slider volumeSlider;
    public Text volumeText;

    void Start()
    {
        volumeSlider.value = Mathf.RoundToInt(AudioListener.volume * 100);
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        UpdateVolume(volumeSlider.value);
    }

    void UpdateVolume(float value)
    {
        AudioListener.volume = value / 100f;
        volumeText.text = $"{value}%";
    }
}