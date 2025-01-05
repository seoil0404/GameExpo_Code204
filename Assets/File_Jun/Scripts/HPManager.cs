using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public Slider playerHPSlider;
    public Slider enemyHPSlider;

    private int playerMaxHP = 10;
    private int enemyMaxHP = 10;
    private int playerCurrentHP;
    private int enemyCurrentHP;

    void Start()
    {
        
        playerCurrentHP = playerMaxHP;
        enemyCurrentHP = enemyMaxHP;

        
        playerHPSlider.maxValue = playerMaxHP;
        enemyHPSlider.maxValue = enemyMaxHP;

        UpdateHPUI();
    }

   
    private void UpdateHPUI()
    {
        playerHPSlider.value = playerCurrentHP;
        enemyHPSlider.value = enemyCurrentHP;
    }

            
    public void TakeDamage(bool isPlayer, int damage)
    {
        if (isPlayer)
        {
            playerCurrentHP = Mathf.Max(playerCurrentHP - damage, 1);
        }
        else
        {
            enemyCurrentHP = Mathf.Max(enemyCurrentHP - damage, 2);
        }

        UpdateHPUI();
    }
}
