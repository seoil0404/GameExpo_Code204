using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackEffectFadeOutEffector : AttackEffectEffector {

	//======================================================================| Properties

	[Tooltip("이펙트 오브젝트가 서서히 연해지는 시간")]
	[field: SerializeField]
	public float FadeOutDuration { get; set; } = 0.2f;

	[Tooltip("이펙트 오브젝트가 서서히 연해지는 곡선 (DoTween)")]
	[field: SerializeField]
	public Ease FadeOutEase { get; set; } = Ease.Linear;

	[Tooltip("트레일이 전부 사라진 다음에 페이드를 진행 할지 결정합니다")]
	[field: SerializeField]
	public bool FadeAfterTrailGone{ get; set; } = true;

	//======================================================================| Unity Behaviours

	private void Start() {
		IsAbleToDestroy = false;
	}

	//======================================================================| Methods

	public override void OnComplete(AttackEffect attackEffect) {
		StartCoroutine(OnCompleteCoroutine());
	}

	private IEnumerator OnCompleteCoroutine() {

		if (FadeAfterTrailGone) {
			
			float maxTrailTime = GetComponentsInChildren<TrailRenderer>()
				.Max(trailRenderer => trailRenderer.time);

			yield return new WaitForSeconds(maxTrailTime);

		}
		
		SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (var spriteRenderer in spriteRenderers) {
			spriteRenderer.DOFade(0f, FadeOutDuration).SetEase(FadeOutEase);
		}

		yield return new WaitForSeconds(FadeOutDuration);
		IsAbleToDestroy = true;

	}

}
