using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioClip normalBGM;
    public AudioClip bossBGM;

    private AudioSource audioSource;
    private AudioClip currentClip;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
    }

    private void Update()
    {
        if (StatisticsManager.Instance == null) return;

        AudioClip desiredClip = StatisticsManager.Instance.CurrentRoom < 15 ? normalBGM : bossBGM;

        if (currentClip != desiredClip)
        {
            currentClip = desiredClip;
            audioSource.clip = currentClip;
            audioSource.Play();
        }
    }
}