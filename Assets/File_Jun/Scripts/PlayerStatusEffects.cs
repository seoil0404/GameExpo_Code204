using UnityEngine;

public class PlayerStatusEffects : MonoBehaviour
{
    public bool PlayerStrength = false;
    public bool PlayerPoison = false;
    public bool PlayerWeak = false;
    public bool PlayerThoron = false;
    public bool PlayerHealingReduction = false;
    public bool PlayerSilenced = false;
    public bool PlayerInvincible = false;

    private CharacterData characterData;

    private void Awake()
    {
        characterData = CharacterManager.selectedCharacter.characterData;
    }

    private void Update()
    {
        UpdateStatusEffects();
    }

    private void UpdateStatusEffects()
    {
        EnemyStats[] enemies = FindObjectsByType<EnemyStats>(FindObjectsSortMode.None);
        PlayerStrength = characterData.CurrentCharacterATK > 0;

        PlayerPoison = false;
        foreach (var enemy in enemies)
        {
            if (enemy.GetPoisonDuration() > 0)
            {
                PlayerPoison = true;
                break;
            }
        }

        PlayerWeak = false;
        foreach (var enemy in enemies)
        {
            if (enemy.isDamageMultiplierActive == true)
            {
                PlayerWeak = true;
                break;
            }
        }

       if(CharacterManager.instance.reflectDamage > 0)
        {
            PlayerThoron = true;
        }


        if(characterData.IsInvincible == true)
        {
            PlayerInvincible = true;
        }    
    }
}
