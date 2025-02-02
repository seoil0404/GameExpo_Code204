using DG.Tweening;
using UnityEngine;

public class AttackEffectSpawner : MonoBehaviour {

	//======================================================================| Properties

	/// <summary>
	/// 공격 이펙스 시작 (출발) 위치
	/// </summary>
	[field: SerializeField]
	public Transform StartPosition { get; set; }

	/// <summary>
	/// 공격 이펙트 목표 (도착) 위치
	/// </summary>
	[field: SerializeField]
	public Transform TargetPosition { get; set; }

	/// <summary>
	/// 날아갈 이펙트 오브젝트 (복사 후 발사)
	/// </summary>
	[field: SerializeField]
	public GameObject ParticleObject { get; set; }

	/// <summary>
	/// 이펙트 오브젝트가 대상까지 날아가는 시간
	/// </summary>
	[field: SerializeField]
	public float FlightTime { get; set; }

	/// <summary>
	/// 이펙트 오브젝트가 날아갈 경로의 높이 (베지어 곡선 2번째 점)
	/// </summary>
	[field: SerializeField]
	public float CurveHeight { get; set; } = 2f;

	/// <summary>
	/// 곡선 커브 (ToDween)
	/// </summary>
	[field: SerializeField]
	public Ease FlightEase { get; set; } = Ease.Linear;

	//======================================================================| Events

	/// <summary>
	/// 이펙트가 목표 지점에 도착하면 실행할 이벤트
	/// </summary>
	[field: SerializeField]
	public event OnCompleateHandler OnCompleate;

	//======================================================================| Fields

	private Canvas ThisCanvas;

	//======================================================================| Unity Behaviours

	private void Awake() {
		ThisCanvas = transform.GetComponentInParent<Canvas>();
	}

	//======================================================================| Methods

	public void Spawn() {
		
		GameObject instantiated = Instantiate(ParticleObject, ThisCanvas.transform);
		instantiated.transform.position = StartPosition.position;
		
		SlerpParticleObject(instantiated);

	}

	private void SlerpParticleObject(GameObject instantiated) {
		
		float progress = 0f;

		Vector3 start = StartPosition.position;
		Vector3 target = TargetPosition.position;

		Vector2 midPoint =
			(start + target) / 2f + // 두 점의 평균 (중앙)
			Vector3.up * CurveHeight; // 중점에다 높이만큼 위로 높힘
		
		DOTween.To(() => progress, x => progress = x, 1f, FlightTime)
			.SetEase(FlightEase)
			.onUpdate = () => {

			// To 서일. 이거 Slerp쓰면 위로 갈지 아래로 갈지 결정 못해서 베지어로 함
			Vector3 position = Bezier(progress, start, midPoint, target);
			Vector3 nextPosition = Bezier(progress + 0.001f, start, midPoint, target);
			Vector3 direction = (nextPosition - position).normalized;

			instantiated.transform.position = position;

			if (direction != Vector3.zero) {
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				instantiated.transform.rotation = Quaternion.Euler(0f, 0f, angle);
			}

		};

		OnCompleate?.Invoke();
		instantiated.transform.DOKill();
		Destroy(instantiated);

	}

	// 베지어 곡선
	private static Vector3 Bezier(float t, Vector3 p1, Vector3 p2, Vector3 p3) =>
		(1 - t) * (1 - t) * p1 + 2 * (1 - t) * t * p2 + t * t * p3;

	//======================================================================| Nested Types

	public delegate void OnCompleateHandler();

}
