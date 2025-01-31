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
}