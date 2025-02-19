using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private GoldData goldData;

    public void Initialize()
    {
        goldData.InGameGold = 0;
    }
}
