using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AttackEffect : MonoBehaviour {

	//======================================================================| Properties

	[Tooltip("이펙트 오브젝트가 대상까지 날아가는 시간")]
	[field: SerializeField]
	public float FlightTime { get; set; } = 1f;

	[Tooltip("곡선 커브 (ToDween)")]
	[field: SerializeField]
	public Ease FlightEase { get; set; } = Ease.Linear;

	[Tooltip("공격 목표 오차")]
	[field: SerializeField]
	public float AttackRange { get; set; } = 0f;

	public AttackEffectMovement MovementMethods { get; set; }
	
	public AttackEffectEffector[] Effectors { get; set; }

	//======================================================================| Unity Behaviours

	private void Awake() {
		MovementMethods = GetComponent<AttackEffectMovement>();
		Effectors = GetComponents<AttackEffectEffector>();
	}

	//======================================================================| Methods

	public void Shoot(GameObject receiverObject, GameObject casterObject, Action onAttack) {

		Vector2 targetPosition = receiverObject.transform.position;

		Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, receiverObject.transform.position);

		if (receiverObject.TryGetComponent<Image>(out var image)) {
			screenPos.y += image.rectTransform.sizeDelta.y / 2;
		}

		// 월드 좌표로 변환
		RectTransform canvasRect = receiverObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
		RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, screenPos, Camera.main, out Vector3 worldPos);
		targetPosition = worldPos;

		if (AttackRange != 0f) {
			targetPosition.x += Random.Range(-AttackRange, AttackRange) / 2f;
			targetPosition.y += Random.Range(-AttackRange, AttackRange) / 2f;
		}
		
		float progress = 0f;
		Vector2 start = transform.position;

		Effectors.OnShootAll(this);

		RotateCaster(receiverObject, casterObject);

		DOTween.To(() => progress, x => progress = x, 1f, FlightTime)
			.SetEase(FlightEase)
			.OnUpdate(() => {
				MovementMethods.Move(this, start, targetPosition, progress);
				Effectors.OnFlyingAll(this);
			})
			.OnComplete(() => {
				HitEffectManager.Instance.OnHit(receiverObject, targetPosition, casterObject);
				Effectors.OnCompleteAll(this);
				StartCoroutine(DestroyOnReady());
				onAttack?.Invoke();
			});

	}

	private void RotateCaster(GameObject reseiverObject, GameObject casterObject) {
				
		float attackRotationAngle = HitEffectManager.Instance.AttackRotationAngle;
		float attackRotationDuration = HitEffectManager.Instance.AttackRotationDuration;

		bool isFlipped = 
			reseiverObject.transform.position.x <
			casterObject.transform.position.x;

		casterObject.transform.DOKill();
		casterObject.transform.rotation = Quaternion.identity;

		float angle = attackRotationAngle * (isFlipped ? -1f : 1f);
		Vector3 endRotation = Vector3.forward * angle;

		casterObject.transform
			.DORotate(endRotation, attackRotationDuration / 3f)
			.SetEase(Ease.OutSine)
			.OnComplete(() => casterObject.transform
				.DORotate(Vector3.zero, attackRotationDuration / 1.5f)
				.SetEase(Ease.InOutSine)
			);

	}

	private IEnumerator DestroyOnReady() {
		
		if (!Effectors.Any()) Destroy(gameObject);

		while (true) {

			if (Effectors.All(effector => effector.IsAbleToDestroy))
				break;

			yield return null;

		}

		Destroy(gameObject);

	}

}