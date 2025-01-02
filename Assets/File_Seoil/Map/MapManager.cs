using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private KeyManager keyManager;
    
    private bool isMapEnable = false;
    public bool IsMapEnable
    {
        get
        {
            return isMapEnable;
        }
    }
    void Update()
    {
        HandleMapState();
    }
    private void HandleMapState()
    {
        if (Input.GetKeyDown(keyManager.MapKey))
        {
            if (map.activeSelf)
            {
                DisableMap();
            }
            else
            {
                EnableMap();
            }
        }
    }
    public void EnableMap()
    {
        map.SetActive(true);
    }
    public void DisableMap()
    {
        map.SetActive(false);
    }
}