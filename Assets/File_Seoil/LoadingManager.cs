using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Image loadingImage;
    [SerializeField] private float fadeDuration;

    public float FadeDuration => fadeDuration;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        loadingImage.color = new Color(loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, 0);
        
        loadingImage.DOKill();
        loadingImage.DOFade(1, fadeDuration);
        
        StartCoroutine(AutoFade());
    }

    private IEnumerator AutoFade()
    {
        yield return new WaitForSeconds(fadeDuration);
        
        loadingImage.DOKill();
        loadingImage.DOFade(0, fadeDuration);

        StartCoroutine(AutoDestroy());
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(fadeDuration);
        Destroy(gameObject);
    }
}
