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
            Debug.LogWarning("HealthText.text�� ���ڰ� �ƴ� ���� ��� �ֽ��ϴ�!");
    }

    public void UpdateHpBar()
    {
        if (!float.TryParse(HealthText.text, out float currentHp))
            Debug.LogWarning("HealthText.text�� ���ڰ� �ƴ� ���� ��� �ֽ��ϴ�!");
        CurrentHpBar.fillAmount = currentHp / MaxHp;
    }
}