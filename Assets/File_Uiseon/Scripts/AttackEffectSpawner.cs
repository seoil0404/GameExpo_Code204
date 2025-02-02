using DG.Tweening;
using UnityEngine;

public class AttackEffectSpawner : MonoBehaviour {

	//======================================================================| Properties

	/// <summary>
	/// ���� ���彺 ���� (���) ��ġ
	/// </summary>
	[field: SerializeField]
	public Transform StartPosition { get; set; }

	/// <summary>
	/// ���� ����Ʈ ��ǥ (����) ��ġ
	/// </summary>
	[field: SerializeField]
	public Transform TargetPosition { get; set; }

	/// <summary>
	/// ���ư� ����Ʈ ������Ʈ (���� �� �߻�)
	/// </summary>
	[field: SerializeField]
	public GameObject ParticleObject { get; set; }

	/// <summary>
	/// ����Ʈ ������Ʈ�� ������ ���ư��� �ð�
	/// </summary>
	[field: SerializeField]
	public float FlightTime { get; set; }

	/// <summary>
	/// ����Ʈ ������Ʈ�� ���ư� ����� ���� (������ � 2��° ��)
	/// </summary>
	[field: SerializeField]
	public float CurveHeight { get; set; } = 2f;

	/// <summary>
	/// � Ŀ�� (ToDween)
	/// </summary>
	[field: SerializeField]
	public Ease FlightEase { get; set; } = Ease.Linear;

	//======================================================================| Events

	/// <summary>
	/// ����Ʈ�� ��ǥ ������ �����ϸ� ������ �̺�Ʈ
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
			(start + target) / 2f + // �� ���� ��� (�߾�)
			Vector3.up * CurveHeight; // �������� ���̸�ŭ ���� ����
		
		DOTween.To(() => progress, x => progress = x, 1f, FlightTime)
			.SetEase(FlightEase)
			.onUpdate = () => {

			// To ����. �̰� Slerp���� ���� ���� �Ʒ��� ���� ���� ���ؼ� ������� ��
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

	// ������ �
	private static Vector3 Bezier(float t, Vector3 p1, Vector3 p2, Vector3 p3) =>
		(1 - t) * (1 - t) * p1 + 2 * (1 - t) * t * p2 + t * t * p3;

	//======================================================================| Nested Types

	public delegate void OnCompleateHandler();

}
