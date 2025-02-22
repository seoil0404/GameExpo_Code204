using UnityEngine;

public class RestRoom : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;
    public float HealAmount = 0.4f;
    public void HealCharacter()
    {
        characters[GameData.SelectedCharacterIndex - 1].characterData.CurrentHp += (int)(characters[GameData.SelectedCharacterIndex - 1].characterData.MaxHp * HealAmount);
        Debug.Log($"�ִ�ü���� {HealAmount}��ŭ ȸ��");

        Scene.Controller.OnClearScene();
        characters[GameData.SelectedCharacterIndex - 1].characterData.OnHpChanged = null;
        Debug.Log("�޽Ĺ� ����");
    }
}