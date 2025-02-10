using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AttackEffect))]

public abstract class AttackEffectMovement : MonoBehaviour {

	public abstract void Move(AttackEffect attackEffect, Vector2 start, Vector2 target, float progress);

}