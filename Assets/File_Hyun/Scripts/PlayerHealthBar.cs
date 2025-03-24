using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;
    public Text Health;
    
    public Image CurrentHpBar;
    public Image DamageInflicted;
    public Image heeledHp;

    public GameObject Executable;

    private int FirstHp;

    void Start()
    {
        StartCoroutine(DelayedStartLogic());

    }

    
    void OnDestroy()
    {
        if (characters != null && characters.Length > 0 && characters[GameData.SelectedCharacterIndex - 1] != null)
        {
            characters[GameData.SelectedCharacterIndex - 1].characterData.OnHpChanged -= UpdateHpBar;
        }
    }

    void UpdateHpBar()
    {
        float current = characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp;
        float max = characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp;
        float first = FirstHp;
        DamageInflicted.fillAmount = first / max;
        Health.text = characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp + " / " + characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp;
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

        if(characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp <= (characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp * characters[GameData.SelectedCharacterIndex - 1].characterData.ExecutionRate) / 100)
        {
            if (GameData.SelectedCharacterIndex == 3)
                Executable.SetActive(true);
        }
    }

    private IEnumerator DelayedStartLogic()
    {
        yield return null;
        Executable.SetActive(false);
        heeledHp.fillAmount = 0;
        FirstHp = characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp;
        //characters[GameData.SelectedCharacterIndex - 1].characterData.OnHpChanged = UpdateHpBar;
        characters[GameData.SelectedCharacterIndex - 1].characterData.OnHpChanged += UpdateHpBar;
        UpdateHpBar();
    }
}