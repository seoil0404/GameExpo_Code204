using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private GoldData goldData;
    [SerializeField] private CombatData combatData;
    [SerializeField] private ScrollData scrollData;

    public void Initialize()
    {
        Debug.Log("[Initiazer] Initialize");

        goldData.InGameGold = 0;
        combatData.Initialize();
        scrollData.Initialize();
    }
}
