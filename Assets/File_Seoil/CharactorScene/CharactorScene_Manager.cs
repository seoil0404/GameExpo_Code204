using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharactorScene_Manager : MonoBehaviour
{
    [SerializeField] private UnityEvent onESCPressed;

    [Header("MonoBehaviour")]
    [SerializeField] private Image characterImage;
    [SerializeField] private Canvas characterTextCanvas;

    [Header("Prefabs")]
    [SerializeField] private GameObject characterTextPrefab;

    [Header("Sprite")]
    [SerializeField] private Sprite Isa;
    [SerializeField] private Sprite Blayrin;
    [SerializeField] private Sprite Hugh;
    [SerializeField] private Sprite IsaTextImage;
    [SerializeField] private Sprite BlayrinTextImage;
    [SerializeField] private Sprite HughTextImage;

    [Header("Sound")]
    public AudioSource audioSource;
    [SerializeField] private AudioClip IsaSound;
    [SerializeField] private AudioClip BlayrinSound;
    [SerializeField] private AudioClip HughSound;
    [SerializeField] private AudioClip EscSound;

    [Header("Animation Setting")]
    [SerializeField] private float characterTextStartInterval;
    [SerializeField] private float characterTextAnimationDuration;

    private GameObject currentCharacterText;
    private Image currentCharacterTextImage;

    private void Awake()
    {
        SetCharacter(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlaySound(EscSound);
            onESCPressed?.Invoke();
        }
    }

    public void SetCharacter(int characterIndex)
    {
        GameData.SelectedCharacterIndex = characterIndex;

        if(currentCharacterText != null) Destroy(currentCharacterText);
        currentCharacterText = Instantiate(characterTextPrefab, characterTextCanvas.transform);

        currentCharacterTextImage = currentCharacterText.GetComponent<Image>();

        Vector2 originalPos = currentCharacterText.GetComponent<RectTransform>().anchoredPosition;

        currentCharacterText.GetComponent<RectTransform>().position = new Vector2(
            currentCharacterText.GetComponent<RectTransform>().anchoredPosition.x - characterTextStartInterval, 
            currentCharacterText.GetComponent<RectTransform>().anchoredPosition.y
            );

        currentCharacterText.GetComponent<RectTransform>().DOMoveX(originalPos.x, characterTextAnimationDuration);

        switch (characterIndex)
        {
            case 1:
                characterImage.sprite = Isa;
                currentCharacterTextImage.sprite = IsaTextImage;
                PlaySound(IsaSound);
                break;
            case 2:
                characterImage.sprite = Blayrin;
                currentCharacterTextImage.sprite = BlayrinTextImage;
                PlaySound(BlayrinSound);
                break;
            case 3:
                characterImage.sprite = Hugh;
                currentCharacterTextImage.sprite = HughTextImage;
                PlaySound(HughSound);
                break;
        }

        characterImage.transform.DOShakePosition(0.1f, 7);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource 또는 AudioClip이 설정되지 않았습니다.");
        }
    }
}
