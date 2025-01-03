using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public Character[] characters;
    private Character selectedCharacter;

    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
    }

    public void StartGame()
    {
        if (selectedCharacter == null)
        {
            return;
        }

        GameData.SelectedCharacterIndex = System.Array.IndexOf(characters, selectedCharacter) + 1;
        SceneManager.LoadScene("GameScene");
    }
}
