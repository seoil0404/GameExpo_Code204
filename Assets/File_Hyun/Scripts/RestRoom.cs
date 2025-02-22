using UnityEngine;

public class RestRoom : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;
    public float HealAmount = 0.4f;
    public void HealCharacter()
    {
        characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp += (int)(characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp * HealAmount);
        Debug.Log($"최대체력의 {HealAmount}만큼 회복");

        Scene.Controller.OnClearScene();
        characters[GameData.SelectedCharacterIndex - 1].characterData.OnHpChanged = null;
        Debug.Log("휴식방 종료");
    }
}