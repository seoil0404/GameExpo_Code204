using UnityEngine;

[System.Serializable]
public class CharacterData : MonoBehaviour
{
    public static CharacterData Instance { get; private set; } //채현

    public string characterName;
    public int ultimateGaugeMax;
    public string ultimateSkillDescription;
    public GameObject characterPrefab;

    public int maxHp;
    public int currentHp;
    public int currentUltimateGauge;

    private void Awake() //채현
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

    public void Initialize(int initialMaxHp)
    {
        maxHp = initialMaxHp;
        currentHp = maxHp;
        currentUltimateGauge = 0;
    }

    public int GetMaxHp() //채현
    {
        return maxHp;
    }

    public void SetMaxHp(int value) //채현
    {
        maxHp = value;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }

    public int GetCurrentHp() //채현
    {
        return currentHp;
    }

    public void SetCurrentHp(int value) //채현
    {
        currentHp = value;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        else if (currentHp < 0)
        {
            currentHp = 0;
        }
    }
}