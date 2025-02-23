using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;
    public Text HealthText;
    public Image CurrentHpBar;

    public GameObject Executable;

    private float MaxHp;

    void Start()
    {
        Executable.SetActive(false);
        if (!float.TryParse(HealthText.text, out MaxHp))
        {
            Debug.LogWarning("HealthText.text에 숫자가 아닌 값이 들어 있습니다!");
        }
    }

    public void UpdateHpBar()
    {
        if (!float.TryParse(HealthText.text, out float currentHp))
        {
            Debug.LogWarning("HealthText.text에 숫자가 아닌 값이 들어 있습니다!");
        }
        CurrentHpBar.fillAmount = currentHp / MaxHp;
        if (currentHp <= (MaxHp * characters[GameData.SelectedCharacterIndex - 1].characterData.ExecutionRate) / 100)
        {
            if (GameData.SelectedCharacterIndex == 3)
                Executable.SetActive(true);
        }
    }
}