using UnityEngine;
using static EnemyData;

public class BackGroundController : MonoBehaviour
{
    public CombatData combatData;
    public GameObject level_1;
    public GameObject level_2;
    public GameObject level_3;

    private void Start()
    {
        transform.SetAsFirstSibling();

        level_1.SetActive(false);
        level_2.SetActive(false);
        level_3.SetActive(false);

        switch (combatData.HabitatType)
        {
            case HabitatType.Forest:
                level_1.SetActive(true);
                break;
            case HabitatType.Castle:
                level_2.SetActive(true);
                break;
            case HabitatType.DevilCastle:
                level_3.SetActive(true);
                break;
            default:
                Debug.LogWarning("Unknown Habitat Type!");
                break;
        }
    }
}