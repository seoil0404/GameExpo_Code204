using UnityEngine;

[CreateAssetMenu(fileName = "KeyManager", menuName = "Scriptable Objects/KeyManager")]
public class KeyManager : ScriptableObject
{
    //===============================================================| Field
    [SerializeField] private KeyCode mapKey;

    //===============================================================| Property
    public KeyCode MapKey
    {
        get { return mapKey; }
        set { mapKey = value; }
    }
}
