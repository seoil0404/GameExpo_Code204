using UnityEngine;

public class AttackEffectMoveCurvy : AttackEffectMovement {

	//======================================================================| Properties

	[Tooltip("이펙트 커브 경로의 높이")]
	[field: SerializeField]
	public float CurveHeight { get; set; } = 2f;

	[Tooltip("이펙트가 날아가며 진행방향을 바라볼지 결정")]
	[field: SerializeField]
	public bool LookFront { get; set; } = true;

	//======================================================================| Methods

	public override void Move(AttackEffect attackEffect, Vector2 start, Vector2 target, float progress) {
		
		Vector2 centerPoint = (start + target) / 2f + Vector2.up * CurveHeight;
		Vector2 position = Bezier(progress, start, centerPoint, target);

		attackEffect.transform.position = position;

		if (LookFront) {
			Vector2 nextPosition = Bezier(progress + 0.001f, start, centerPoint, target);
			Vector2 direction = (position - nextPosition).normalized;

			if (direction != Vector2.zero) {
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0f, 0f, angle);
			}
		}
	}

	private static Vector2 Bezier(float t, Vector2 p1, Vector2 p2, Vector2 p3) =>
        (1 - t) * (1 - t) * p1 + 2 * (1 - t) * t * p2 + t * t * p3;

}