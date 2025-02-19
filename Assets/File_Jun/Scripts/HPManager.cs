using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public static HPManager Instance { get; private set; } 
    
    public Slider playerHPSlider;
    public Slider enemyHPSlider;

    private int playerMaxHP = 10;
    private int enemyMaxHP = 10;
    private int playerCurrentHP;
    private int enemyCurrentHP;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
            playerCurrentHP = Mathf.Max(playerCurrentHP - damage, 0);
            if (playerCurrentHP <= 0)
            {
                Debug.Log("ÇÃ·¹ÀÌ¾î »ç¸Á!");
            }
        }
        else
        {
            enemyCurrentHP = Mathf.Max(enemyCurrentHP - damage, 0);
            if (enemyCurrentHP <= 0)
            {
                Debug.Log("Àû »ç¸Á!");
            }
        }

        UpdateHPUI();
    }

}
