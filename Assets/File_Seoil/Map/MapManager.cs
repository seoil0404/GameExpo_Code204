using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Canvas map;
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
        HandleMapState(map);
    }
    private void HandleMapState(Canvas mapCanvas)
    {
        if (Input.GetKeyDown(keyManager.MapKey))
        {
            if (mapCanvas.gameObject.activeSelf)
            {
                DisableMap(mapCanvas);
            }
            else
            {
                EnableMap(mapCanvas);
            }
        }
    }
    public void EnableMap(Canvas mapCanvas)
    {
        mapCanvas.gameObject.SetActive(true);
    }
    public void DisableMap(Canvas mapCanvas)
    {
        mapCanvas.gameObject.SetActive(false);
    }
}