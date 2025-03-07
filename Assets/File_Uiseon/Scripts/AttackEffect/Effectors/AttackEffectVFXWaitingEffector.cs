using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class AttackEffectVFXWaitingEffect : AttackEffectEffector {

	//======================================================================| Unity Methods

	private void Start() {
		IsAbleToDestroy = false;
	}

	//======================================================================| Methods

	public override void OnComplete(AttackEffect attackEffect) {
		StartCoroutine(OnCompleteCoroutine());
	}

	private IEnumerator OnCompleteCoroutine() {
	
		VisualEffect[] visualEffects = GetComponentsInChildren<VisualEffect>();
		yield return new WaitUntil(() => visualEffects.Any(visualEffect => visualEffect.aliveParticleCount == 0));
		IsAbleToDestroy = true;

	}

}