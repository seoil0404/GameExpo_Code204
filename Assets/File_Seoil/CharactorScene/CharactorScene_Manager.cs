using UnityEngine;
using UnityEngine.UI;

public class CharactorScene_Manager : MonoBehaviour
{
    [Header("MonoBehaviour")]
    [SerializeField] private Image characterImage;

    [Header("Sprite")]
    [SerializeField] private Sprite aisa;
    [SerializeField] private Sprite blarin;

    private void Awake()
    {
        MatchCharacter();
    }

    public void SetCharacter(int characterIndex)
    {
        GameData.SelectedCharacterIndex = characterIndex;
        MatchCharacter();
    }

    private void MatchCharacter()
    {
        switch (GameData.SelectedCharacterIndex)
        {
            case 1:
                characterImage.sprite = aisa;
                //characterImage.transform.localScale = Vector3.one;
                break;
            case 2:
                characterImage.sprite = blarin;
                //characterImage.transform.localScale = Vector3.one * -1;
                break;
        }
    }
}
