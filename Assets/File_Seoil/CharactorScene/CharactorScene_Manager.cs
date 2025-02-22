using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharactorScene_Manager : MonoBehaviour
{
    [SerializeField] private UnityEvent onESCPressed;

    [Header("MonoBehaviour")]
    [SerializeField] private Image characterImage;

    [Header("Sprite")]
    [SerializeField] private Sprite Isa;
    [SerializeField] private Sprite Blayrin;
    [SerializeField] private Sprite Hugh;

    [Header("Sound")]
    public AudioSource audioSource;
    [SerializeField] private AudioClip IsaSound;
    [SerializeField] private AudioClip BlayrinSound;
    [SerializeField] private AudioClip HughSound;
    [SerializeField] private AudioClip EscSound;

    private void Awake()
    {
        GameData.SelectedCharacterIndex = 1;
        characterImage.sprite = Isa;
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
        switch (characterIndex)
        {
            case 1:
                characterImage.sprite = Isa;
                PlaySound(IsaSound);
                break;
            case 2:
                characterImage.sprite = Blayrin;
                PlaySound(BlayrinSound);
                break;
            case 3:
                characterImage.sprite = Hugh;
                PlaySound(HughSound);
                break;
        }
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
