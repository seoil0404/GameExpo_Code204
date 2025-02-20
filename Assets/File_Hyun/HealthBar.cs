using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //[SerializeField] private Character[] characters = null;
    public Text Health;

    public Image CurrentHpBar;
    public Image DamageInflicted;
    public Image heeledHp;

    private int MaxHp = 100; //characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp
    private int CurrentHp = 60; //characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp
    private int FirstHp;

    void Start()
    {
        heeledHp.fillAmount = 0;
        FirstHp = CurrentHp;
        UpdateHpBar();
    }

    public void IncreaseMaxHp()
    {
        MaxHp++;
        UpdateHpBar();
    }

    public void DecreaseMaxHp()
    {
        MaxHp--;
        UpdateHpBar();
    }

    public void IncreaseCurrentHp()
    {
        CurrentHp++;
        UpdateHpBar();
    }

    public void DecreaseCurrentHp()
    {
        CurrentHp--;
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        Health.text = CurrentHp + " / " + MaxHp;
        float current = CurrentHp, max = MaxHp, first = FirstHp;
        DamageInflicted.fillAmount = first / max;
        if (CurrentHp > FirstHp)
        {
            CurrentHpBar.fillAmount = first / max;
            heeledHp.fillAmount = current / max;
        }
        else
        {
            heeledHp.fillAmount = 0;
            CurrentHpBar.fillAmount = current / max;
        }
    }
}