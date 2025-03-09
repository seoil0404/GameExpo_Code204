using UnityEngine;
using UnityEngine.UI;

public class ShopRoom_BackGroundManager : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private CombatData combatData;

    [Header("MonoBehavior")]
    [SerializeField] private Image backGround;

    [Header("Images")]
    [SerializeField] private Sprite level1BackGroundImage;
    [SerializeField] private Sprite level2BackGroundImage;
    [SerializeField] private Sprite level3BackGroundImage;
    private void Awake()
    {
        switch(combatData.HabitatType)
        {
            case EnemyData.HabitatType.Forest:
                backGround.sprite = level1BackGroundImage;
                break;
            case EnemyData.HabitatType.Castle:
                backGround.sprite = level2BackGroundImage;
                break;
            case EnemyData.HabitatType.DevilCastle:
                backGround.sprite = level3BackGroundImage;
                break;
            default:
                Debug.LogError("Unknown Habitat type");
                break;
        }
    }
}
