using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnClick : MonoBehaviour
{
    [SerializeField] private Button myButton;
    [SerializeField] private float cooldownTime = 1f;

    private void Start()
    {
        myButton.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        Debug.Log("On Click");
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        myButton.interactable = false;
        yield return new WaitForSeconds(cooldownTime);
        myButton.interactable = true;
    }
}