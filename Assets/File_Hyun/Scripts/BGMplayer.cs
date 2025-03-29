using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMPlayer : MonoBehaviour
{
    public float BGM_volume = 0.3f;

    public AudioClip normalBGM;
    public AudioClip bossBGM;

    private AudioSource audioSource;
    private AudioClip currentClip;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = BGM_volume;
    }

    private void Update()
    {
        if (StatisticsManager.Instance == null) return;

        bool isBossCondition = StatisticsManager.Instance.CurrentRoom > 14 &&
                               UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene";

        AudioClip desiredClip = isBossCondition ? bossBGM : normalBGM;

        if (currentClip != desiredClip)
        {
            currentClip = desiredClip;
            audioSource.clip = currentClip;
            audioSource.Play();
        }
    }
}