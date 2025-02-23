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
            Debug.LogWarning("HealthText.text�� ���ڰ� �ƴ� ���� ��� �ֽ��ϴ�!");
        }
    }

    public void UpdateHpBar()
    {
        if (!float.TryParse(HealthText.text, out float currentHp))
        {
            Debug.LogWarning("HealthText.text�� ���ڰ� �ƴ� ���� ��� �ֽ��ϴ�!");
        }
        CurrentHpBar.fillAmount = currentHp / MaxHp;
        if (currentHp <= (MaxHp * characters[GameData.SelectedCharacterIndex - 1].characterData.ExecutionRate) / 100)
        {
            if (GameData.SelectedCharacterIndex == 3)
                Executable.SetActive(true);
        }
    }
}