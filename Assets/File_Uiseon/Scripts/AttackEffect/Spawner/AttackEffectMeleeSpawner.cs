using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class AttackEffectMeleeSpawner : AttackEffectSpawner {

	//======================================================================| Properties

	[field: Header("Approach")]
	[field: SerializeField]
	public Ease ApproachEase { get; set; }

	[field: SerializeField]
	public float ApproachDuraion { get; set; }

	[field: SerializeField]
	public float GapBetweenTarget { get; set; }

	[field: Header("Retreat")]
	[field: SerializeField]
	public float RetreatWaitingDuration { get; set; }

	[field: SerializeField]
	public Ease RetreatEase { get; set; }

	[field: SerializeField]
	public float RetreatDuraion { get; set; }

	[field: Header("Attack")]
	[field: SerializeField]
	public float AttackWaitingDuration { get; set; }

	[field: SerializeField]
	public float AttackingDuration { get; set; }

	[field: SerializeField]
	public float AttackingAngle { get; set; }

	[field: SerializeField]
	public VisualEffect AttackEffect { get; set; }

	//======================================================================| Fields

	private Transform casterTransform;
	private Vector2 endPosition;

	//======================================================================| Methods

	public override void Spawn() {
	
		casterTransform = transform.parent;
		endPosition = Vector2.Lerp(
			casterTransform.position, 
			TargetTransform.position,
			1 - GapBetweenTarget
		);

		Approach();

	}

	private void Approach() {
		casterTransform
			.DOMove(endPosition, ApproachDuraion)
			.SetEase(ApproachEase)
			.OnComplete(() => StartCoroutine(AttackStart()));
	}

	private IEnumerator AttackStart() {
		
		yield return new WaitForSeconds(AttackWaitingDuration);

		bool isFlipped =
			startPosition.x <
			TargetTransform.position.x;

		float angle = AttackingAngle * (isFlipped ? -1 : 1);
		Vector3 endRotation = Vector3.forward * angle;

		PlayAnimation();

		casterTransform
			.DORotate(endRotation, AttackingDuration / 3f)
			.SetEase(Ease.OutSine)
			.OnComplete(() => AttackEnd());

	}

	private void PlayAnimation() {

		VisualEffect attackEffect = Instantiate(AttackEffect);
		attackEffect.transform.position = transform.position;

		StartCoroutine(WaitAndDestroyEffect(attackEffect));

		HitEffectManager.Instance.OnHit(
			TargetTransform.gameObject,
			TargetTransform.position,
			transform.parent.gameObject
		);

	}

	private IEnumerator WaitAndDestroyEffect(VisualEffect instantiated) {
		yield return new WaitUntil(() => instantiated.aliveParticleCount == 0);
		Destroy(instantiated);
	}

	private void AttackEnd() {
		
		casterTransform
			.DORotate(Vector3.zero, AttackingDuration / 1.5f)
			.OnComplete(() => StartCoroutine(Retreat()));

	}

	private IEnumerator Retreat() {
		
		yield return new WaitForSeconds(RetreatWaitingDuration);

		casterTransform
			.DOMove(startPosition, RetreatDuraion)
			.SetEase(RetreatEase);
	}

}