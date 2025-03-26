using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResetTextEffect : MonoBehaviour
{
    public Text resetText;
    private float duration = 0.3f;

    private Tween currentTween;

    private void Start()
    {
        resetText.gameObject.SetActive(false);
    }

    public void OnResetButtonClicked()
    {
        // 기존 트윈 중지
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        resetText.text = "RESET!";
        resetText.gameObject.SetActive(true);
        resetText.color = new Color(resetText.color.r, resetText.color.g, resetText.color.b, 0.9f);
        resetText.transform.localScale = Vector3.one;

        currentTween = DOTween.Sequence()
            .Append(resetText.transform.DOScale(0.6f, duration).SetEase(Ease.OutCubic))
            .Join(resetText.DOFade(0f, duration).SetEase(Ease.InQuad))
            .OnComplete(() =>
            {
                resetText.gameObject.SetActive(false);
                currentTween = null;
            });
    }

    private void Update()
    {
        if (currentTween != null && currentTween.IsActive() && Input.GetMouseButtonDown(0))
        {
            currentTween.Kill();
            resetText.gameObject.SetActive(false);
            currentTween = null;
        }
    }
}