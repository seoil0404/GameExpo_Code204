using System.Collections.Generic;
using UnityEngine;

public abstract class AttackEffectEffector : MonoBehaviour {

	public bool IsAbleToDestroy { get; protected set; }

	public virtual void OnShoot(AttackEffect attackEffect) {}
	public virtual void OnFlying(AttackEffect attackEffect) {}
	public virtual void OnComplete(AttackEffect attackEffect) {}

}

public static class AttackEffectEffectorArrayExtension {

	public static void OnShootAll(this IEnumerable<AttackEffectEffector> effectors, AttackEffect attackEffect) {
		foreach (var effector in effectors) {
			effector.OnShoot(attackEffect);
		}
	}

	public static void OnFlyingAll(this IEnumerable<AttackEffectEffector> effectors, AttackEffect attackEffect) {
		foreach (var effector in effectors) {
			effector.OnFlying(attackEffect);
		}
	}

	public static void OnCompleteAll(this IEnumerable<AttackEffectEffector> effectors, AttackEffect attackEffect) {
		foreach (var effector in effectors) {
			effector.OnComplete(attackEffect);
		}
	}

}