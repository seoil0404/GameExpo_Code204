using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public int maxHp;
    public int ultimateGaugeMax;
    public string ultimateSkillDescription;
    public GameObject characterPrefab;

    [HideInInspector]
    public int currentHp;
    [HideInInspector]
    public int currentUltimateGauge;

    public void Initialize()
    {
        currentHp = maxHp;
        currentUltimateGauge = 0;
    }

    public int GetMaxHp() //ä��
    {
        return maxHp;
    }

    public void SetMaxHp(int value) //ä��
    {
        maxHp = value;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }

    public int GetCurrentHp() //ä��
    {
        return currentHp;
    }

    public void SetCurrentHp(int value) //ä��
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