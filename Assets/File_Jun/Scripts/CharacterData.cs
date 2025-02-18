using UnityEngine;

[System.Serializable]
public class CharacterData : MonoBehaviour
{
    public static CharacterData Instance { get; private set; } //ä��

    public string characterName;
    public int ultimateGaugeMax;
    public string ultimateSkillDescription;
    public GameObject characterPrefab;

    public int maxHp;
    public int currentHp;
    public int currentUltimateGauge;

    private void Awake() //ä��
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