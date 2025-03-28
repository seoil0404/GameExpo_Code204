using UnityEngine;
using UnityEngine.UI;
using static EnemyData;

public class BackGroundController : MonoBehaviour
{
    public CombatData combatData;
    public Image backgroundImage;

    public Sprite forestBackground;
    public Sprite castleBackground;
    public Sprite devilCastleBackground;

    private void Start()
    {
        transform.SetAsFirstSibling();

        switch (combatData.HabitatType)
        {
            case HabitatType.Forest:
                backgroundImage.sprite = forestBackground;
                break;
            case HabitatType.Castle:
                backgroundImage.sprite = castleBackground;
                break;
            case HabitatType.DevilCastle:
                backgroundImage.sprite = devilCastleBackground;
                break;
            default:
                Debug.LogWarning("Unknown Habitat Type!");
                break;
        }
    }
}