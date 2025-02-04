using UnityEngine;

public class AttackEffectMoveStraight : AttackEffectMovement {

	//======================================================================| Properties

	[Tooltip("이펙트가 날아가며 진행방향을 바라볼지 결정")]
	[field: SerializeField]
	public bool LookFront { get; set; } = true;

	//======================================================================| Fields

	private bool isLookingFront = false;

	//======================================================================| Methods

	public override void Move(AttackEffect attackEffect, Vector2 start, Vector2 target, float progress) {

		if (!isLookingFront) {
			Vector2 direction = (start - target).normalized;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, angle);
			isLookingFront = true;
		}

		Vector2 position = Vector2.Lerp(start, target, progress);
		attackEffect.transform.position = position;

	}

}