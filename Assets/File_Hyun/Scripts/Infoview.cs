using UnityEngine;
using UnityEngine.UI;

public class Infoview : MonoBehaviour
{
    [SerializeField] private Character[] characters = null;

    public Text Name;
    public Text Floor;


    void Start()
    {
        if (Name != null) Name.text = characters[GameData.SelectedCharacterIndex - 1].characterData.CharacterName;
        if(Floor != null) Floor.text = $"[ {StatisticsManager.Instance.CurrentFloor}   /   {StatisticsManager.Instance.CurrentRoom + 1} ]";
    }
}
