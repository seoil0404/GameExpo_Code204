using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class AttackEffectMeleeSpawner : AttackEffectSpawner {

	//======================================================================| Properties

	[field: Header("Approach")]
	[field: SerializeField]
	public Ease ApproachEase { get; set; } = Ease.OutCubic;

	[field: SerializeField]
	public float ApproachDuraion { get; set; } = 0.2f;

	[field: SerializeField]
	public float GapBetweenTarget { get; set; } = 0.2f;

	[field: Header("Retreat")]
	[field: SerializeField]
	public float RetreatWaitingDuration { get; set; } = 0f;

	[field: SerializeField]
	public Ease RetreatEase { get; set; } = Ease.OutCubic;

	[field: SerializeField]
	public float RetreatDuraion { get; set; } = 0.2f;

	[field: Header("Attack")]
	[field: SerializeField]
	public float AttackWaitingDuration { get; set; } = 0.1f;

	[field: SerializeField]
	public float AttackingDuration { get; set; } = 0.2f;

	[field: SerializeField]
	public float AttackingAngle { get; set; } = 15f;

	[field: SerializeField]
	public VisualEffect AttackEffect { get; set; }

	//======================================================================| Fields

	private Transform casterTransform;
	private Vector2 endPosition;

	//======================================================================| Methods

	public override void Spawn(Action onAttack = null) {
	
		casterTransform = transform.parent;
		endPosition = Vector2.Lerp(
			casterTransform.position, 
			new(TargetTransform.position.x, casterTransform.position.y),
			1 - GapBetweenTarget
		);

		Approach(onAttack);

	}

	private void Approach(Action onAttack) {
		casterTransform
			.DOMove(endPosition, ApproachDuraion)
			.SetEase(ApproachEase)
			.OnComplete(() => StartCoroutine(AttackStart(onAttack)));
	}

	private IEnumerator AttackStart(Action onAttack) {
		
		Debug.Log($"testLog: {casterTransform.gameObject.name} Attack Start Started");
		yield return new WaitForSeconds(AttackWaitingDuration);

		bool isFlipped =
			startPosition.x <
			TargetTransform.localPosition.x;

		Debug.Log($"testLog: {casterTransform.gameObject.name} break 1");

		float angle = AttackingAngle * (isFlipped ? -1 : 1);
		Vector3 endRotation = Vector3.forward * angle;

		Debug.Log($"testLog: {casterTransform.gameObject.name} break 2");

		PlayAnimation();
		onAttack?.Invoke();

		Debug.Log($"testLog: {casterTransform.gameObject.name} break 3");

		casterTransform
			.DORotate(endRotation, AttackingDuration / 3f)
			.SetEase(Ease.OutSine)
			.OnKill(() => AttackEnd())
			.OnComplete(() => AttackEnd());

	}

	private void PlayAnimation() {

		if (AttackEffect != null) {

			VisualEffect attackEffect = Instantiate(AttackEffect);
			attackEffect.transform.position = transform.position;

		}

		EffectManager.Instance.OnHit(
			TargetTransform.gameObject,
			TargetTransform.position,
			transform.parent.gameObject
		);

	}


	private void AttackEnd() {

		Debug.Log($"testLog: {casterTransform.gameObject.name} Start Attack End");

		casterTransform
			.DORotate(Vector3.zero, AttackingDuration / 1.5f)
			.OnComplete(() => {
				Debug.Log($"testLog: {casterTransform.gameObject.name} Attack End Complete");	
                StartCoroutine(Retreat());
			});

	}

	private IEnumerator Retreat() {
		
		yield return new WaitForSeconds(RetreatWaitingDuration);

		Debug.Log($"testLog: {casterTransform.gameObject.name} Start Retreat [CurrentPos: {casterTransform.position}, TargetPosition: {startPosition}]");

        casterTransform
			.DOMove(startPosition, RetreatDuraion)
			.SetEase(RetreatEase)
			.OnComplete(() => Debug.Log($"testLog: {casterTransform.gameObject.name} Retreat Complete"));
    }

}