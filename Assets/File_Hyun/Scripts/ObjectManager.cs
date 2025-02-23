using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private UnityEvent onESCPressed;
    public GameObject targetObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onESCPressed?.Invoke();
        }
    }

    public void DeactivateObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("targetObject�� �������� �ʾҽ��ϴ�.");
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
            Debug.LogWarning("targetObject�� �������� �ʾҽ��ϴ�.");
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
            Debug.LogWarning("targetObject�� �������� �ʾҽ��ϴ�.");
        }
    }
}
