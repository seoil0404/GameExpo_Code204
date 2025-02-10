using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

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

	public void Shoot(GameObject targetObject) {

		Vector2 targetPosition = targetObject.transform.position;

		if (AttackRange != 0f) {
			targetPosition.x += Random.Range(-AttackRange, AttackRange) / 2f;
			targetPosition.y += Random.Range(-AttackRange, AttackRange) / 2f;
		}
		
		float progress = 0f;
		Vector2 start = transform.position;

		Effectors.OnShootAll(this);

		DOTween.To(() => progress, x => progress = x, 1f, FlightTime)
			.SetEase(FlightEase)
			.OnUpdate(() => {
				MovementMethods.Move(this, start, targetPosition, progress);
				Effectors.OnFlyingAll(this);
			})
			.OnComplete(() => {
				HitEffectManager.Instance.OnHit(targetObject, targetPosition);
				Effectors.OnCompleteAll(this);
				StartCoroutine(DestroyOnReady());
			});

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