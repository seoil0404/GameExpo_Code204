using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackEffectFadeOutEffector : AttackEffectEffector {

	//======================================================================| Properties

	[Tooltip("����Ʈ ������Ʈ�� ������ �������� �ð�")]
	[field: SerializeField]
	public float FadeOutDuration { get; set; } = 0.2f;

	[Tooltip("����Ʈ ������Ʈ�� ������ �������� � (DoTween)")]
	[field: SerializeField]
	public Ease FadeOutEase { get; set; } = Ease.Linear;

	[Tooltip("Ʈ������ ���� ����� ������ ���̵带 ���� ���� �����մϴ�")]
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
