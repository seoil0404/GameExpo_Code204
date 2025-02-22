using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Text HealthText;
    public Image CurrentHpBar;
    private float MaxHp;

    void Start()
    {
        if (!float.TryParse(HealthText.text, out MaxHp))
            Debug.LogWarning("HealthText.text에 숫자가 아닌 값이 들어 있습니다!");
    }

    public void UpdateHpBar()
    {
        if (!float.TryParse(HealthText.text, out float currentHp))
            Debug.LogWarning("HealthText.text에 숫자가 아닌 값이 들어 있습니다!");
        CurrentHpBar.fillAmount = currentHp / MaxHp;
    }
}