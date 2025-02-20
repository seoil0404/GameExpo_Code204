using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance { get; private set; }

    [SerializeField] private Character[] characters = null;
    public Text Health;

    public Image CurrentHpBar;
    public Image DamageInflicted;
    public Image heeledHp;

    private int FirstHp;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        heeledHp.fillAmount = 0;
        FirstHp = characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp;
        characters[GameData.SelectedCharacterIndex - 1].characterData.OnHpChanged += UpdateHpBar;
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        Health.text = characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp + " / " + characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp;
        float current = characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp, max = characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp, first = FirstHp;
        DamageInflicted.fillAmount = first / max;
        if (characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp > FirstHp)
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