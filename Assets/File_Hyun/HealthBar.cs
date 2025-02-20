using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;
    public Text Health;

    public Image CurrentHpBar;
    public Image DamageInflicted;

    private int MaxHp = 100; //characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp
    private int CurrentHp = 60; //characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp
    private int firstHp;

    void Start()
    {
        firstHp = CurrentHp;
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
        CurrentHpBar.fillAmount = (MaxHp / CurrentHp) * 1.0f;
    }
}