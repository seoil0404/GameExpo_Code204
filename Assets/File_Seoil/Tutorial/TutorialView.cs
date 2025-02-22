using UnityEngine;
using UnityEngine.UI;

public class TutorialView : MonoBehaviour
{
    [SerializeField] private Image tutorialImage;

    [SerializeField] private Sprite[] sprites;

    private int currentIndex = 0;

    private void Awake()
    {
        OnClicked();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnClicked();
        }
    }

    private void OnClicked()
    {
        tutorialImage.sprite = sprites[currentIndex];

        currentIndex++;
        if(currentIndex >= sprites.Length) Destroy(gameObject);
    }
}
