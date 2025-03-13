using UnityEngine;
using static EnemyData;

public class RestRoom : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;

    public CombatData combatData;
    public GameObject level_1;
    public GameObject level_2;
    public GameObject level_3;

    public float HealAmount = 0.4f;

    private void Start()
    {
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
    public void HealCharacter()
    {
        characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp += (int)(characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp * HealAmount);
        Debug.Log($"최대체력의 {HealAmount}만큼 회복");

        Scene.Controller.OnClearScene();
        characters[GameData.SelectedCharacterIndex - 1].characterData.OnHpChanged = null;
        Debug.Log("휴식방 종료");
    }
}