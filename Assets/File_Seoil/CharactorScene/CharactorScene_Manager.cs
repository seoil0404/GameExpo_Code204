using UnityEngine;

public class CharactorScene_Manager : MonoBehaviour
{
    public void SetCharacter(int characterIndex)
    {
        GameData.SelectedCharacterIndex = characterIndex;
    }

}
