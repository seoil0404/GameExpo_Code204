using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public int ultimateGaugeMax;
    public string ultimateSkillDescription;
    public GameObject characterPrefab;

    [SerializeField]
    public int maxHp;
    [SerializeField]
    public int currentHp;
    [SerializeField]
    public int currentUltimateGauge;

    public void Initialize()
    {
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