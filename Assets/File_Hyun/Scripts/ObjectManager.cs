using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject targetObject;

    public void DeactivateObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("targetObject가 설정되지 않았습니다.");
        }
    }

    public void ActivateObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("targetObject가 설정되지 않았습니다.");
        }
    }

    public void ToggleObject()
    {
        if (targetObject != null)
        {
            bool isActive = targetObject.activeSelf;
            targetObject.SetActive(!isActive);
        }
        else
        {
            Debug.LogWarning("targetObject가 설정되지 않았습니다.");
        }
    }
}
