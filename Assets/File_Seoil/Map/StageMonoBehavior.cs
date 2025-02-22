using System.Dynamic;
using UnityEngine;

public class StageMonoBehavior : MonoBehaviour
{
    [SerializeField] private GameObject clearImage;

    public MapManager mapManager { set; private get; }

    public void OnCleared()
    {
        Instantiate(clearImage, transform); 
    }

    public void Clicked(GameObject stageObject)
    {
        mapManager.ClickedStage(stageObject);
    }
}
