using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackEffectTrailWaitingEffector : AttackEffectEffector {

	//======================================================================| Unity Behaviours

	private void Start() {
		IsAbleToDestroy = false;
	}

	//======================================================================| Methods

	public override void OnComplete(AttackEffect attackEffect) {
		StartCoroutine(OnCompleteCoroutine());
	}

	private IEnumerator OnCompleteCoroutine() {

		TrailRenderer[] trailRenderers = GetComponentsInChildren<TrailRenderer>();

		float maxTrailTime = trailRenderers
			.Max(trailRenderer => trailRenderer.time);

		yield return new WaitForSeconds(maxTrailTime);
		IsAbleToDestroy = true;

	}

}
