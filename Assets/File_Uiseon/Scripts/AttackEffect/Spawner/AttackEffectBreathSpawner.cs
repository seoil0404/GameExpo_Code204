using System;
using System.Collections;
using UnityEngine;

public class AttackEffectBreathSpawner : AttackEffectSpawner {

	[SerializeField]
	private float _DamageDelay;

	public override void Spawn(Action onAttack = null) {
		StartCoroutine(SpawnInner(onAttack));
	}

	private IEnumerator SpawnInner(Action onAttack) {
		
		EffectManager.Instance.OnDragonBreath(TargetTransform.gameObject, gameObject);
		yield return new WaitForSeconds(_DamageDelay);

		EffectManager.Instance.OnHit(
			TargetTransform.gameObject,
			TargetTransform.position,
			transform.parent.gameObject
		);
		onAttack?.Invoke();

	}

}