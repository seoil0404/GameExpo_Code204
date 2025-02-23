using UnityEngine;
using UnityEngine.Events;

public class EscBack : MonoBehaviour
{
    [SerializeField] private UnityEvent onESCPressed;
    [SerializeField] private AudioClip EscSound;
    public AudioSource audioSource;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlaySound(EscSound);
            onESCPressed?.Invoke();
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
